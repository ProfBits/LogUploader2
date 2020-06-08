﻿using LogUploader.Data;
using LogUploader.Data.Settings;
using LogUploader.GUIs;
using LogUploader.Helper;
using LogUploader.Helper.JobQueue;
using LogUploader.Helpers;
using LogUploader.Languages;
using LogUploader.Properties;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader
{
    class LogUploaderLogic : IDisposable
    {
        internal SettingsData Settings { get; private set; }
        private DPSReport DPSReport { get; set; }
        private JobQueue<CachedLog> Worker { get; set; }
        private CancellationTokenSource WorkerCTS { get; set; }
        private Task WatchDogTask { get; set; }
        private CancellationTokenSource WatchDogCTS { get; set; }
        private WebHookDB WebHookDB { get; set; }

        private readonly object AutoParseLock = new object();
        private readonly object AutoUploadLock = new object();
        private volatile bool enableAutoParsing = false;
        private volatile bool enableAutoUpload = false;

        public bool EnableAutoParsing
        {
            get
            {
                lock(AutoParseLock)
                    return enableAutoParsing;
            }
            set
            {
                lock (AutoParseLock)
                    enableAutoParsing = value;
            }
        }
        public bool EnableAutoUpload
        {
            get
            {
                lock (AutoUploadLock)
                    return enableAutoUpload;
            }
            set
            {
                lock (AutoUploadLock)
                    enableAutoUpload = value;
            }
        }

        #region Events

        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;

        protected virtual void OnDataChanged(EventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        public delegate void JobDoneEventHandler(object sender, JobDoneEventArgs<CachedLog> e);
        public event JobDoneEventHandler JobDone;
        protected virtual void OnJobDone(JobDoneEventArgs<CachedLog> e)
        {
            JobDone?.Invoke(this, e);
        }

        public delegate void QueueEmptyEventHandler(object sender, JobQueueEmptyEventArgs e);
        public event QueueEmptyEventHandler JobsDone;
        protected virtual void OnJobsDone(JobQueueEmptyEventArgs e)
        {
            JobsDone?.Invoke(this, e);
        }

        public delegate void JobAddedEventHandler(object sender, JobAddedEventArgs<CachedLog> e);
        public event JobAddedEventHandler JobAdded;
        protected virtual void OnJobAdded(JobAddedEventArgs<CachedLog> e)
        {
            JobAdded?.Invoke(this, e);
        }

        public delegate void JobStartedEventHandler(object sender, JobStartedEventArgs<CachedLog> e);
        public event JobStartedEventHandler JobStarted;
        protected virtual void OnJobStarted(JobStartedEventArgs<CachedLog> e)
        {
            JobStarted?.Invoke(this, e);
        }

        public delegate void JobFaultedEventHandler(object sender, JobFaultedEventArgs<CachedLog> e);
        public event JobFaultedEventHandler JobFaulted;
        protected virtual void OnJobFaulted(JobFaultedEventArgs<CachedLog> e)
        {
            JobFaulted?.Invoke(this, e);
        }

        private void InitJobQueueEvents(JobQueue<CachedLog> jobQueue)
        {
            jobQueue.JobAdded += (sender, e) => OnJobAdded(e);
            jobQueue.JobDone += (sender, e) => OnJobDone(e);
            jobQueue.JobFaulted += (sender, e) => OnJobFaulted(e);
            jobQueue.JobStarted += (sender, e) => OnJobStarted(e);
            jobQueue.JobQueueEmpty += (sender, e) => OnJobsDone(e);
        }

        #endregion

        public LogUploaderLogic() { }

        public async Task InitLogUploaderLogic(SettingsData settings, bool eap, bool eau, IProgress<ProgressMessage> progress = null)
        {
            progress?.Report(new ProgressMessage(0, "Init"));
            Settings = settings;
            DPSReport = new DPSReport(Settings, settings.UserToken);
            EnableAutoParsing = eap;
            EnableAutoUpload = eau;

            progress?.Report(new ProgressMessage(0.01, "Webhooks"));
            WebHookDB = Settings.WebHookDB;


            progress?.Report(new ProgressMessage(0.03, "Starting worker"));
            WorkerCTS = new CancellationTokenSource();
            Worker = new JobQueue<CachedLog>(WorkerCTS.Token, "LogProcessing JobQueue");
            InitJobQueueEvents(Worker);

            progress?.Report(new ProgressMessage(0.05, "Starting worker"));
            WatchDogCTS = new CancellationTokenSource();
            WatchDogTask = RunWatchDog(settings.ArcLogsPath, WatchDogCTS);

            await Task.Run(() => CheckForNewLogs(new Progress<double>(p => progress?.Report(new ProgressMessage(0.1 + (p * 0.9), "Updating Local Files")))));
        }

        private void CheckForNewLogs(IProgress<double> progress = null)
        {
            progress?.Report(0);
            var newestElement = LogDBConnector.GetNewest();
            var min = newestElement?.Date.Subtract(new TimeSpan(1, 0, 0, 0)) ?? new DateTime(2000, 1, 1);
            var allFiles = Directory.GetFiles(Settings.ArcLogsPath, "*.*", SearchOption.AllDirectories);
            List<DBLog> newLogs = FilterNewLogs(progress, min, allFiles);
            progress?.Report(0.95);
            if (newLogs.Count > 0)
                LogDBConnector.InsertBulk(newLogs);
        }

        private List<DBLog> FilterNewLogs(IProgress<double> progress, DateTime min, string[] allFiles)
        {
            var newLogs = new List<DBLog>();
            var reportStep = 32;
            var numFiles = allFiles.Length;
            for (int i = 0; i < numFiles; i++)
            {
                if (i % reportStep == 0)
                {
                    progress?.Report(0.95 * i / numFiles);
                }
                var path = allFiles[i];
                var Fi = new FileInfo(path);
                if (Fi.LastWriteTime < min || LogDBConnector.GetByEvtcPaht(path).Count() != 0)
                    continue;
                var id = GetBoss(path).ID;
                newLogs.Add(new DBLog(id, path, null, null, null, (int)Math.Ceiling(Fi.Length / 1000.0), Fi.LastWriteTime));
            }

            return newLogs;
        }

        private async Task RunWatchDog(string arcLogsPath, CancellationTokenSource cts)
        {
            var ct = cts.Token;
            await Task.Run(() =>
            {
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = arcLogsPath;

                    watcher.NotifyFilter = NotifyFilters.FileName
                                         | NotifyFilters.DirectoryName;

                    watcher.Filter = "*.*";
                    watcher.IncludeSubdirectories = true;

                    watcher.Created += (sender, e) => FileChangedFilterHandler(e);
                    watcher.Renamed += (sender, e) => FileChangedFilterHandler(e);

                    watcher.EnableRaisingEvents = true;

                    ct.WaitHandle.WaitOne();
                }
            }, ct);
        }
        private void FileChangedFilterHandler(FileSystemEventArgs e)
        {
            if (e.Name.EndsWith(".zevtc") || e.Name.EndsWith(".evtc"))
                Worker.Add(new NamedJob<CachedLog>($"Adding {e.Name}", () => HandelNewFile(e.FullPath)));
        }

        private CachedLog HandelNewFile(string path)
        {
            var absolutPath = path;
            var bossID = GetBoss(path).ID;
            var date = File.GetCreationTime(absolutPath);
            var sizeKb = (int)Math.Ceiling(new FileInfo(absolutPath).Length / 1000.0);

            var newLog = new CachedLog(-1, bossID, absolutPath, null, null, null, sizeKb, date);

            newLog.ID = LogDBConnector.Insert(newLog.GetDBLog());
            var taskName = newLog.BossName.Length > 17 ? newLog.BossName.Substring(0, Math.Min(17, newLog.BossName.Length)) + "..." : newLog.BossName;

            LogCache.Add(newLog);

            OnDataChanged(new EventArgs());

            if (EnableAutoParsing && EnableAutoUpload)
                ParseAndUpload(newLog.ID, "Processing new " + taskName);
            else if (EnableAutoParsing)
                Parse(newLog.ID, "Parsing new " + taskName);
            else if (EnableAutoUpload)
                Upload(newLog.ID, "Uploading new " + taskName);

            GC.Collect();
            return newLog;
        }

        public int Upload(int id, string name = "Uploading")
        {
            var job = new NamedJob<CachedLog>(name, () => {
                var res = UploadJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                GC.Collect();
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }

        private CachedLog UploadJob(CachedLog log)
        {
            if (!string.IsNullOrEmpty(log.Link))
                return log;
            if (string.IsNullOrEmpty(log.EvtcPath) || !File.Exists(log.EvtcPath))
            {
                UpdateFilePaths(log);
                return log;
            }
            var response = DPSReport.UpladContent(log.EvtcPath);
            var jsonData = new JSONHelper.JSONHelper().Desirealize(response);
            if (jsonData.Values.ContainsKey("Error"))
                throw new Exception($"Could not upload the {log.BossName} log! ({log.SizeKb} kb)\n{log.EvtcPath}\n\nResponse: \"{jsonData.GetTypedElement<string>("Error")}\"");
            var link = jsonData.GetTypedElement<string>("permalink");
            if (!log.DataCorrected)
            {
                response = DPSReport.GetEncounterDataPermalink(link);
                jsonData = new JSONHelper.JSONHelper().Desirealize(response);
                log.UpdateEi(jsonData);

                if (string.IsNullOrWhiteSpace(log.JsonPath))
                {
                    var simpleLogJson = new SimpleLogJson(jsonData);
                    var evtcName = Path.GetFileName(log.EvtcPath);
                    var newjson = EliteInsights.LogsPath + evtcName.Substring(0, evtcName.LastIndexOf('.')) + "_simple.json";
                    GP.WriteJsonFile(newjson, simpleLogJson.GetJSONObject().ToString());
                    log.JsonPath = newjson;
                }
            }
            log.Link = link;

            LogDBConnector.Update(log.GetDBLog());

            return log;
        }

        public int Parse(int id, string name = "Parsing")
        {
            var job = new NamedJob<CachedLog>(name, () => {
                var res = ParseJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                GC.Collect();
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }
        private CachedLog ParseJob(CachedLog log)
        {
            if (!string.IsNullOrEmpty(log.HtmlPath))
                return log;
            if (string.IsNullOrEmpty(log.EvtcPath) || !File.Exists(log.EvtcPath))
            {
                UpdateFilePaths(log);
                return log;
            }
            var res = EliteInsights.Parse(log.EvtcPath);
            var html = res.Where(path => path.EndsWith(".html")).FirstOrDefault();
            var json = res.Where(path => path.EndsWith(".json")).FirstOrDefault();
            //Maybe Add corrupted flag if no output is generated
            if (res.Count == 0)
                throw new Exception($"Could not parse the {log.BossName} log! ({log.SizeKb} kb)\n{log.EvtcPath}");
            if (!log.DataCorrected)
            {
                var jsonStr = GP.ReadJsonFile(json);
                var jsonData = new JSONHelper.JSONHelper().Desirealize(jsonStr);

                log.UpdateEi(jsonData);

                if (string.IsNullOrWhiteSpace(log.JsonPath))
                {
                    var simpleLogJson = new SimpleLogJson(jsonData);
                    var newjson = json.Substring(0, json.Length - ".json".Length) + "_simple.json";
                    GP.WriteJsonFile(newjson, simpleLogJson.GetJSONObject().ToString());
                    log.JsonPath = newjson;
                }

            }
            log.HtmlPath = html;

            File.Delete(json);

            LogDBConnector.Update(log.GetDBLog());

            return log;
        }

        public int ParseAndUpload(int id, string name = "Parse and Upload")
        {
            var job = new NamedJob<CachedLog>(name, () => {
                var res = ParseAndUploadJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }

        private CachedLog ParseAndUploadJob(CachedLog log)
        {
            var parse = Task.Run(() => ParseJob(log));
            var upload = Task.Run(() => UploadJob(log));
            Task.WaitAll(parse, upload);
            //ParseJob(log);
            //UploadJob(log);
            return log;
        }

        public CachedLog QuickCacheLog(int id)
        {
            var log = LogCache.getLog(id);
            if (log == null)
            {
                log = new CachedLog(LogDBConnector.GetByID(id));
                LogCache.Add(log);
            }
            if (!File.Exists(log.EvtcPath))
            {
                log.EvtcPath = null;
                LogDBConnector.Update(log.GetDBLog());
            }
            return log;
        }

        private Boss GetBoss(string Path)
        {
            string folder;
            if (Path.EndsWith(".evtc") || Path.EndsWith(".zevtc") || Path.EndsWith(".evtc.zip"))
            {
                folder = GetBossPartFromPath(Path);
            }
            else
                folder = Path.Trim('\\').Split('\\').LastOrDefault();

            if (folder == null)
                return Boss.getByID(0);
            else if (ushort.TryParse(folder, out ushort id))
                return Boss.getByID((int)id);
            else
                return Boss.getByFolderName(folder);
        }

        private static string GetBossPartFromPath(string Path)
        {
            var folders = Path.Trim('\\').Split('\\');
            if (folders.Contains("arcdps.cbtlogs"))
            {
                var index = folders.ToList().FindIndex(s => s == "arcdps.cbtlogs");
                if (index + 1 < folders.Length)
                    return folders[index + 1];

            }
            if (folders.Length >= 2)
                return folders[folders.Length - 2];
            return null;
        }

        internal void ApplySettings()
        {
            var mainSettings = new Settings();
            Settings = new SettingsData(mainSettings);

            Language.Current = Settings.Language;
            EliteInsights.Settings = Settings;
            Helper.DiscordPostGen.DiscordPostGenerator.Settings = Settings;
            WebHookDB = Settings.WebHookDB;
            
            OnDataChanged(new EventArgs());

        }

        #region Copy Links

        public int CopyLinks(System.Windows.Forms.Control invokeHelper, params int[] ids)
        {
            var elements = GetCopyLink(
                ids.Select(id => QuickCacheLog(id))
                .OrderBy(log => log.Date)
                ).ToList();
            string res = string.Join("\n", elements);
            if (!string.IsNullOrWhiteSpace(res))
            {
                Action copy = () => System.Windows.Forms.Clipboard.SetText(res, System.Windows.Forms.TextDataFormat.Text);
                invokeHelper.Invoke(copy);
            }
            return elements.Count();
        }

        private IEnumerable<string> GetCopyLink(IEnumerable<CachedLog> logs)
        {
            foreach (var log in logs)
            {
                if (string.IsNullOrEmpty(log.Link))
                    continue;
                
                var boss = Boss.getByID(log.BossID) ?? Boss.getByID(0);
                var str = "";

                if (Settings.UseGnDiscordEmotes)
                    str += boss.DiscordEmote;
                if (Settings.EncounterName)
                {
                    str += str == "" ? "" : " ";
                    str += boss.Name;
                }
                if (Settings.EncounterSuccess)
                {
                    str += str == "" ? "" : " - ";
                    str += log.Succsess ? Language.Data.Succsess : Language.Data.Fail;
                }
                if (Settings.Inline)
                    str += str == "" ? "" : ": ";
                else
                    str += str == "" ? "" : ":\n";
                str += log.Link.Replace("\\/", "/");
                if (Settings.EmptyLineBetween)
                    str += "\n";
                yield return str;
            }
        }

        #endregion
        #region Open

        public void OpenLocal(params int[] ids)
        {
            foreach (var id in ids)
            {
                var log = QuickCacheLog(id);
                if (string.IsNullOrEmpty(log.HtmlPath))
                    return;
                var link = log.HtmlPath;
                _ = Task.Run(() => System.Diagnostics.Process.Start(link));
            }
        }

        public void OpenLink(params int[] ids)
        {
            foreach (var id in ids)
            {
                var log = QuickCacheLog(id);
                if (string.IsNullOrEmpty(log.Link))
                    continue;
                var link = log.Link.Replace("\\/", "/");
                _ = Task.Run(() => System.Diagnostics.Process.Start(link));
            }
        }

        #endregion
        #region Filter

        public string GetFilter(FilterConfiguration filterConfiguration)
        {
            List<string> filters = new List<string>();
            if (filterConfiguration.BossEnabled)
                filters.Add(BossFilter(filterConfiguration.BossName));
            if (filterConfiguration.DurationEnabled)
                filters.Add(DurationFilter(filterConfiguration.DurationRelation, filterConfiguration.DurationValue));
            if (filterConfiguration.HPEnabled)
                filters.Add(HpFilter(filterConfiguration.HpRelation, filterConfiguration.HpValue));
            if (filterConfiguration.DateFromEnabled)
                filters.Add(DateFromFilter(filterConfiguration.DateFrom));
            if (filterConfiguration.DateFromEnabled)
                filters.Add(DateToFilter(filterConfiguration.DateTo));
            if (filterConfiguration.SuccsessEnabled)
                filters.Add(SuccsessFilter(filterConfiguration.Succsess));

            return string.Join(" AND ", filters);
        }

        private string BossFilter(string name)
        {
            return $"[BossName] LIKE '%{name}%'";
        }

        private string SuccsessFilter(bool succsess)
        {
            var boolean = succsess ? "TRUE" : "FALSE";
            return $"[Succsess] = {boolean}";
        }
        private string HpFilter(string relation, double value)
        {
            return $"[RemainingHealth] {relation} {value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        private string DurationFilter(string relation, TimeSpan value)
        {
            return $"[DurationMs] {relation} {DBLog.getDurationMs(value)}";
        }

        private string DateFromFilter(DateTime value)
        {
            return $"[TimeStamp] >= {DBLog.getTimeStamp(value)}";
        }

        private string DateToFilter(DateTime value)
        {
            return $"[TimeStamp] <= {DBLog.getTimeStamp(value)}";
        }

        public Tuple<DateTime, DateTime> TodayFilterBorders()
        {
            var today = DateTime.Now.Date;
            return new Tuple<DateTime, DateTime>(today, today.AddDays(1));
        }

        public readonly string[] FilterRelations = { "<", "<=", "=", ">=", ">" };
        
        #endregion
        #region WebHookHandling

        public List<WebHook> GetWebHooks()
        {
            return WebHookDB.GetWebHooks();
        }

        public long GetSelectedWebhook()
        {
            if (WebHookDB.IDExists(Settings.CurrentWebHook))
                return Settings.CurrentWebHook;
            if (WebHookDB.HasEntries())
                return WebHookDB.GetWebHooks().Min(wh => wh.ID);
            return WebHookDB.GetWebHooks().First().ID;
        }

        public void SetSelectedWebhook(long id)
        {
            if (!WebHookDB.IDExists(id))
                return;

            Settings.CurrentWebHook = id;
            var mainSettings = new Settings();
            mainSettings.CurrentWebHook = id;
            mainSettings.Save();
        }

        public async Task PostToDiscord(long webHookID, params int[] ids)
        {
            var webHook = WebHookDB[webHookID];
            if (webHook == null)
                return;

            //Maybe: Move post() to WebHook -> eliminate WebHookHelper

            var logs = ids.Select(id => CacheLog(id)).ToArray();
            var posts = webHook.GetPosts(logs);
            try {
                await WebHookHelper.PostWebHookPosts(webHook, posts, Settings);
            }
            catch (WebException e)
            {
                //TODO localize
                MessageBox.Show($"Unable to Post to Webhook {webHook.Name}.\n" +
                    $"Make sure\n" +
                    $"- the address of the WebHook is correct\n" +
                    $"- if an avatar is specified check its link\n" +
                    $"- you are connected to the internet",
                    "Discord posting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        public DataTable GetData()
        {
            var data = LogDBConnector.GetAll();
            var tabel = ListToDataTable.Convert(data);
            return tabel;
        }

        public int GetElementCount()
        {
            return LogDBConnector.GetCount();
        }

        public LogPreview GetFullPreview(CancellationToken token, params int[] ids)
        {
            if (ids.Count() != 1)
                return GetQuickPreview(token, ids);
            List<CachedLog> logs = new List<CachedLog>();
            foreach (var id in ids)
            {
                if (token.IsCancellationRequested) return null;
                var log = CacheLog(id);
                if (log != null) logs.Add(log);
            }
            if (token.IsCancellationRequested) return null;
            return GetPreview(token, logs);
        }

        public LogPreview GetQuickPreview(CancellationToken token, params int[] ids)
        {
            List<CachedLog> logs = new List<CachedLog>();
            foreach (var id in ids)
            {
                if (token.IsCancellationRequested) return null;
                var log = QuickCacheLog(id);
                if (log != null) logs.Add(log);
            }
            if (token.IsCancellationRequested) return null;
            return GetPreview(token, logs);
        }

        private LogPreview GetPreview(CancellationToken token, List<CachedLog> logs)
        {
            if (logs.Count == 1)
            {
                var log = logs[0];
                var pData = log.Players?.OrderByDescending(p => p.DPS).Select(p => getPlayerData(p));
                return new LogPreview(log, pData.Count() > 0 ? pData : null);
            }
            if (logs.Count > 1)
            {
                string name = "";
                DateTime date = DateTime.Now;
                CheckState Corrected = CheckState.Indeterminate;
                CheckState IsCM = CheckState.Indeterminate;
                CheckState Success = CheckState.Indeterminate;
                CheckState HasHtml = CheckState.Indeterminate;
                CheckState HasLink = CheckState.Indeterminate;

                var names = logs.Select(log => log.BossName).Distinct().ToList();
                if (names.Count == 1)
                    name = names.First();
                else
                    name = Language.Data.MiscDetailsMultibleBosses;

                if (token.IsCancellationRequested) return null;

                var size = logs.Sum(log => log.SizeKb);
                var dates = logs.Select(log => log.Date.Date).Distinct().ToList();

                if (token.IsCancellationRequested) return null;

                if (dates.Count >= 1)
                    date = dates.First();
                TimeSpan totalDuration = new TimeSpan(0);
                foreach (var log in logs)
                {
                    totalDuration = totalDuration.Add(log.Duration);
                }
                var maxHP = logs.Max(log => log.RemainingHealth);

                if (token.IsCancellationRequested) return null;

                if (logs.All(log => log.DataCorrected))
                    Corrected = CheckState.Checked;
                else if (logs.All(log => !log.DataCorrected))
                    Corrected = CheckState.Unchecked;

                if (token.IsCancellationRequested) return null;

                if (logs.All(log => log.IsCM))
                    IsCM = CheckState.Checked;
                else if (logs.All(log => !log.IsCM))
                    IsCM = CheckState.Unchecked;

                if (token.IsCancellationRequested) return null;

                if (logs.All(log => log.Succsess))
                    Success = CheckState.Checked;
                else if (logs.All(log => !log.Succsess))
                    Success = CheckState.Unchecked;

                if (token.IsCancellationRequested) return null;

                if (logs.All(log => !string.IsNullOrWhiteSpace(log.HtmlPath)))
                    HasHtml = CheckState.Checked;
                else if (logs.All(log => string.IsNullOrWhiteSpace(log.HtmlPath)))
                    HasHtml = CheckState.Unchecked;

                if (token.IsCancellationRequested) return null;

                if (logs.All(log => !string.IsNullOrWhiteSpace(log.Link)))
                    HasLink = CheckState.Checked;
                else if (logs.All(log => string.IsNullOrWhiteSpace(log.Link)))
                    HasLink = CheckState.Unchecked;

                if (token.IsCancellationRequested) return null;

                return new LogPreview(name, size, maxHP, date, true, totalDuration, Corrected, IsCM,
                        Success, HasHtml, true, "", HasLink, "", null);
            }
            return new LogPreview("", 0, 0, DateTime.Now, false, new TimeSpan(0), CheckState.Indeterminate, CheckState.Indeterminate,
                CheckState.Indeterminate, CheckState.Indeterminate, false, null, CheckState.Indeterminate, null, null);
        }

        private PlayerData getPlayerData(CachedPlayer player)
        {
            var p = new PlayerData();
            p.Width = 143;
            p.Margin = new Padding(0, 1, 0, 1);

            p.ClassImage = player.ProfessionIcon;
            p.DisplayName = player.AccountName.TrimEnd("0123456789.".ToCharArray());
            p.SubGroup = player.SubGroup.ToString();
            p.DPS = player.DPS.ToString();

            return p;
        }

        public CachedLog CacheLog(int id)
        {
            var log = QuickCacheLog(id);
            if (!string.IsNullOrWhiteSpace(log.JsonPath) && ((log.Players?.Count ?? 0) == 0))
            {
                if (!File.Exists(log.JsonPath))
                    return log;
                var jsonStr = GP.ReadJsonFile(log.JsonPath);
                var jsonData = new JSONHelper.JSONHelper().Desirealize(jsonStr);
                var simpleLogJson = new SimpleLogJson(jsonData);
                log.Players = simpleLogJson.Players
                    .Select(p => new CachedPlayer(p.Account, p.Name, Profession.Get(p.Profession), (byte) p.Group, p.DpsAll))
                    .ToList();
            }
            GC.Collect();
            LogCache.Add(log);
            return log;
        }

        public void UpdateFilePaths(CachedLog log)
        {
            if (string.IsNullOrEmpty(log.EvtcPath) || !File.Exists(log.EvtcPath))
                log.EvtcPath = null;
            if (string.IsNullOrEmpty(log.HtmlPath) || !File.Exists(log.HtmlPath))
                log.HtmlPath = null;
            if (string.IsNullOrEmpty(log.JsonPath) || !File.Exists(log.JsonPath))
                log.JsonPath = null;
            LogDBConnector.Update(log.GetDBLog());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    WatchDogCTS.Cancel();
                    WorkerCTS.Cancel();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LogUploaderLogic()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

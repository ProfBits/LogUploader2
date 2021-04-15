using Extensiones.Linq;
using LogUploader.Data;
using LogUploader.Data.Discord;
using LogUploader.Data.RaidOrgaPlus;
using LogUploader.GUIs;
using LogUploader.Helper;
using LogUploader.Tools.JobQueue;
using LogUploader.Localisation;
using LogUploader.Properties;
using LogUploader.Tools.Discord;
using LogUploader.Tools.Discord.DiscordPostGen;
using LogUploader.Tools.DpsReport;
using LogUploader.Tools.Logging;
using LogUploader.Tools.Settings;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Tools;
using LogUploader.Tools.Database;
using LogUploader.Tools.RaidOrgaPlus;

namespace LogUploader
{
    class LogUploaderLogic : IDisposable
    {
        public SettingsData Settings { get; private set; }
        private DPSReport DPSReport { get; set; }
        private JobQueue<ICachedLog> Worker { get; set; }
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

        private RaidOrgaPlusConnector RaidOrgaPlusConnector = null;
        private Session RaidOrgaPlusSession = null;
        private List<RaidSimple> RaidOrgaPlusTermine = new List<RaidSimple>();

        #region Events

        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;

        protected virtual void OnDataChanged(EventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        public delegate void JobDoneEventHandler(object sender, JobDoneEventArgs<ICachedLog> e);
        public event JobDoneEventHandler JobDone;
        protected virtual void OnJobDone(JobDoneEventArgs<ICachedLog> e)
        {
            JobDone?.Invoke(this, e);
        }

        public delegate void QueueEmptyEventHandler(object sender, JobQueueEmptyEventArgs e);
        public event QueueEmptyEventHandler JobsDone;
        protected virtual void OnJobsDone(JobQueueEmptyEventArgs e)
        {
            JobsDone?.Invoke(this, e);
        }

        public delegate void JobAddedEventHandler(object sender, JobAddedEventArgs<ICachedLog> e);
        public event JobAddedEventHandler JobAdded;
        protected virtual void OnJobAdded(JobAddedEventArgs<ICachedLog> e)
        {
            JobAdded?.Invoke(this, e);
        }

        public delegate void JobStartedEventHandler(object sender, JobStartedEventArgs<ICachedLog> e);
        public event JobStartedEventHandler JobStarted;
        protected virtual void OnJobStarted(JobStartedEventArgs<ICachedLog> e)
        {
            JobStarted?.Invoke(this, e);
        }

        public delegate void JobFaultedEventHandler(object sender, JobFaultedEventArgs<ICachedLog> e);
        public event JobFaultedEventHandler JobFaulted;
        protected virtual void OnJobFaulted(JobFaultedEventArgs<ICachedLog> e)
        {
            JobFaulted?.Invoke(this, e);
        }

        private void InitJobQueueEvents(JobQueue<ICachedLog> jobQueue)
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
            EnableAutoParsing = settings.EnableAutoParse || eap;
            EnableAutoUpload = settings.EnableAutoUpload || eau;

            progress?.Report(new ProgressMessage(0.01, "Webhooks"));
            WebHookDB = Settings.WebHookDB;


            progress?.Report(new ProgressMessage(0.03, "Starting worker"));
            WorkerCTS = new CancellationTokenSource();
            Worker = new JobQueue<ICachedLog>(WorkerCTS.Token, "LogProcessing JobQueue");
            InitJobQueueEvents(Worker);

            progress?.Report(new ProgressMessage(0.05, "Starting worker"));
            WatchDogCTS = new CancellationTokenSource();
            WatchDogTask = RunWatchDog(settings.ArcLogsPath, WatchDogCTS);

            progress?.Report(new ProgressMessage(0.06, "RO+"));
            await Task.Run(() => LoadTermine(new Progress<ProgressMessage>(p => progress?.Report(new ProgressMessage((0.24 * p.Percent) + 0.06, "RO+ " + p.Message)))));
            
            await Task.Run(() => UpdateUnkowen(new Progress<double>(p => progress?.Report(new ProgressMessage(0.3 + (p * 0.2), "Updating Local Files Old")))));

            await Task.Run(() => CheckForNewLogs(new Progress<double>(p => progress?.Report(new ProgressMessage(0.5 + (p * 0.5), "Updating Local Files New")))));
        }

        private void UpdateUnkowen(IProgress<double> progress = null)
        {
            progress?.Report(0);
            var logs = LogDBConnector.GetByBossIdWithPath(0);
            int i = 0;
            int count = logs.Count;
            foreach (var log in logs)
            {
                bool update = false;
                var id = GetBoss(log.EvtcPath).ID;
                if (id != 0)
                {
                    update = true;
                    log.BossID = id;
                }
                if (!File.Exists(log.EvtcPath))
                {
                    update = true;
                    log.EvtcPath = null;
                }
                if (update)
                {
                    LogDBConnector.Update(log);
                }
                progress?.Report((double)i++/count);
                i++;
            }
        }

        private void CheckForNewLogs(IProgress<double> progress = null)
        {
            progress?.Report(0);
            var newestElement = LogDBConnector.GetNewest();
            var min = newestElement?.Date.Subtract(new TimeSpan(1, 0, 0, 0)) ?? new DateTime(2000, 1, 1);
            var allFiles = Directory.EnumerateFiles(Settings.ArcLogsPath, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(".evtc") || file.EndsWith(".zevtc") || file.EndsWith(".evtc.zip"))
                .ToList();
            List<IDBLog> newLogs = FilterNewLogs(progress, min, allFiles).Cast<IDBLog>().ToList();
            progress?.Report(0.95);
            if (newLogs.Count > 0)
                LogDBConnector.InsertBulk(newLogs);
        }

        private List<DBLog> FilterNewLogs(IProgress<double> progress, DateTime min, List<string> allFiles)
        {
            var newLogs = new List<DBLog>();
            var reportStep = 32;
            var numFiles = allFiles.Count;
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
                Worker.Add(new NamedJob<ICachedLog>($"Adding {e.Name}", () => HandelNewFile(e.FullPath)));
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
                ParseAndUpload(newLog.ID, $"{Language.Data.FooterProcessing} {taskName}");
            else if (EnableAutoParsing)
                Parse(newLog.ID, $"{Language.Data.FooterParsing} {taskName}");
            else if (EnableAutoUpload)
                Upload(newLog.ID, $"{Language.Data.FooterUploading} {taskName}");

            GC.Collect();
            return newLog;
        }

        public int Upload(int id, string name = "Uploading")
        {
            var job = new NamedJob<ICachedLog>(name, () => {
                var res = UploadJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                GC.Collect();
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }

        private ICachedLog UploadJob(ICachedLog log)
        {
            var localDataVersion = CacheLog(log.ID).DataVersion;
            if (!string.IsNullOrEmpty(log.Link) && localDataVersion >= CachedLog.CurrentDataVersion)
                return log;
            if (string.IsNullOrEmpty(log.EvtcPath) || !File.Exists(log.EvtcPath))
            {
                UpdateFilePaths(log);
                return log;
            }
            var response = DPSReport.UpladContent(log.EvtcPath);
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(response);
            if (jsonData.ContainsKey("Error"))
            {
                var ex = new Exception($"Could not upload the {log.BossName} log! ({log.SizeKb} kb)\n{log.EvtcPath}\n\nResponse: \"{(string)jsonData["Error"]}\"");
                if (log.SizeKb >= 30)
                    throw ex;
                Logger.LogException(ex);
                return log;
            }
            var link = (string)jsonData["permalink"];
            if (!log.DataCorrected || localDataVersion < CachedLog.CurrentDataVersion)
            {
                response = DPSReport.GetEncounterDataPermalink(link);
                log.UpdateEi(response);

                if (string.IsNullOrWhiteSpace(log.JsonPath))
                {
                    var simpleLogJson = new SimpleLogJson(response);
                    var evtcName = Path.GetFileName(log.EvtcPath);
                    var newjson = EliteInsights.LogsPath + evtcName.Substring(0, evtcName.LastIndexOf('.')) + "_simple.json";
                    JsonHandling.WriteJsonFile(newjson, simpleLogJson.ToString());
                    log.JsonPath = newjson;
                }
            }
            log.Link = link;

            LogDBConnector.Update(log.GetDBLog());

            return log;
        }

        public int Parse(int id, string name = "Parsing")
        {
            var job = new NamedJob<ICachedLog>(name, () => {
                var res = ParseJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                GC.Collect();
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }
        private ICachedLog ParseJob(ICachedLog log)
        {
            var localDataVersion = CacheLog(log.ID).DataVersion;
            if (!string.IsNullOrEmpty(log.HtmlPath) && localDataVersion >= CachedLog.CurrentDataVersion)
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
            {
                var ex = new Exception($"Could not parse the {log.BossName} log! ({log.SizeKb} kb)\n{log.EvtcPath}");
                if (log.SizeKb >= 30)
                    throw ex;
                Logger.LogException(ex);
                return log;
            }
            if (!log.DataCorrected || localDataVersion < CachedLog.CurrentDataVersion)
            {
                var jsonStr = JsonHandling.ReadJsonFile(json);
                log.UpdateEi(jsonStr);

                if (string.IsNullOrWhiteSpace(log.JsonPath))
                {
                    var simpleLogJson = new SimpleLogJson(jsonStr);
                    var newjson = json.Substring(0, json.Length - ".json".Length) + "_simple.json";
                    JsonHandling.WriteJsonFile(newjson, simpleLogJson.ToString());
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
            var job = new NamedJob<ICachedLog>(name, () => {
                var res = ParseAndUploadJob(QuickCacheLog(id));
                OnDataChanged(new EventArgs());
                return res;
            });
            Worker.Add(job);
            return job.ID;
        }

        private ICachedLog ParseAndUploadJob(ICachedLog log)
        {
            var parse = Task.Run(() => ParseJob(log));
            var upload = Task.Run(() => UploadJob(log));
            try
            {
                Task.WaitAll(parse, upload);
            }
            catch (AggregateException e)
            {
                throw new AggregateException($"{e.InnerExceptions.Count()} Exeptions occured\n" +
                    $"First:\n" +
                    $"{e.InnerExceptions.FirstOrDefault()?.Message}\n" +
                    $"{e.InnerExceptions.FirstOrDefault()?.StackTrace}", e.InnerExceptions);
            }
            //ParseJob(log);
            //UploadJob(log);
            return log;
        }

        private ICachedLog ReParseData(ICachedLog log)
        {
            if (!log.DataCorrected)
                return ParseJob(log);

            if (!string.IsNullOrWhiteSpace(log.Link))
            {
                var response = DPSReport.GetEncounterDataPermalink(log.Link);
                var simpleLogJson = new SimpleLogJson(response);

                if (!string.IsNullOrWhiteSpace(log.JsonPath))
                {
                    if (File.Exists(log.JsonPath)) File.Delete(log.JsonPath);
                }
                string name;
                if (!string.IsNullOrWhiteSpace(log.EvtcPath))
                    name = log.EvtcPath.Substring(0, log.EvtcPath.LastIndexOf('.')).Split('\\').Last();
                else
                    name = log.Link.Split('/').Last();
                var newjson = EliteInsights.LogsPath + name + "_simple.json";
                JsonHandling.WriteJsonFile(newjson, simpleLogJson.ToString());
                log.JsonPath = newjson;
                LogDBConnector.Update(log.GetDBLog());
                log.ApplySimpleLog(simpleLogJson);
                return log;
            }
            if (!string.IsNullOrWhiteSpace(log.EvtcPath))
            {
                if (!File.Exists(log.EvtcPath))
                {
                    UpdateFilePaths(log);
                    return log;
                }
                var res = EliteInsights.Parse(log.EvtcPath);
                var html = res.Where(path => path.EndsWith(".html")).FirstOrDefault();
                var json = res.Where(path => path.EndsWith(".json")).FirstOrDefault();
                if (res.Count == 0)
                    return null;

                if (!string.IsNullOrWhiteSpace(log.HtmlPath) && File.Exists(log.HtmlPath))
                    File.Delete(log.HtmlPath);
                if (!string.IsNullOrWhiteSpace(log.JsonPath) && File.Exists(log.JsonPath))
                    File.Delete(log.HtmlPath);
                if (!log.DataCorrected)
                {
                    var jsonStr = JsonHandling.ReadJsonFile(json);
                    log.UpdateEi(jsonStr);

                    if (string.IsNullOrWhiteSpace(log.JsonPath))
                    {
                        var simpleLogJson = new SimpleLogJson(jsonStr);
                        var newjson = json.Substring(0, json.Length - ".json".Length) + "_simple.json";
                        JsonHandling.WriteJsonFile(newjson, simpleLogJson.ToString());
                        log.JsonPath = newjson;
                    }

                }
                log.HtmlPath = html;

                File.Delete(json);

                LogDBConnector.Update(log.GetDBLog());
            }
            return null; // cannot upgrade data
        }

        public ICachedLog QuickCacheLog(int id)
        {
            var log = LogCache.GetLog(id);
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
                return Boss.GetByID(0);
            else if (ushort.TryParse(folder, out ushort id))
                return Boss.GetByID((int)id);
            else
                return Boss.GetByFolderName(folder);
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
            Settings = SettingsData.Load();

            Language.SetLanguage(Settings.Language);
            EliteInsights.Settings = Settings;
            DiscordPostGenerator.Settings = Settings;
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

        private IEnumerable<string> GetCopyLink(IEnumerable<ICachedLog> logs)
        {
            foreach (var log in logs)
            {
                if (string.IsNullOrEmpty(log.Link))
                    continue;
                
                var boss = Boss.GetByID(log.BossID) ?? Boss.GetByID(0);
                var str = "";

                if (Settings.UseGnDiscordEmotes)
                    str += boss.DiscordEmote;
                if (Settings.EncounterName)
                {
                    str += string.IsNullOrEmpty(str) ? "" : " ";
                    str += boss.Name;
                }
                if (Settings.EncounterSuccess)
                {
                    str += string.IsNullOrEmpty(str) ? "" : " - ";
                    str += log.Succsess ? Language.Data.Succsess : Language.Data.Fail;
                }
                if (Settings.Inline)
                    str += string.IsNullOrEmpty(str) ? "" : ": ";
                else
                    str += string.IsNullOrEmpty(str) ? "" : ":\n";
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
            return $"[DurationMs] {relation} {DBLog.GetDurationMs(value)}";
        }

        private string DateFromFilter(DateTime value)
        {
            return $"[TimeStamp] >= {DBLog.GetTimeStamp(value)}";
        }

        private string DateToFilter(DateTime value)
        {
            return $"[TimeStamp] <= {DBLog.GetTimeStamp(value)}";
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
            var settings = SettingsData.Load();
            settings.CurrentWebHook = id;
            settings.Save();
        }

        public async Task PostToDiscord(long webHookID, params int[] ids)
        {
            var webHook = WebHookDB[webHookID];
            if (webHook == null)
                return;

            //Maybe: Move post() to WebHook -> eliminate WebHookHelper

            var logs = ids.Select(id => CacheLog(id)).ToArray();
            var generator = DiscordPostGenerator.Get(webHook.Format);
            var posts = generator.Generate(logs, webHook.Name, webHook.AvatarURL);
            try {
                await WebHookHelper.PostWebHookPosts(webHook, posts, Settings);
            }
            catch (WebException e)
            {
                Logger.Error("Exception in: LogUploaderLogic.PostToDiscord");
                Logger.LogException(e);
                if (e.InnerException != null)
                {
                    Logger.Error("Inner Exception:");
                    Logger.LogException(e);
                }

                MessageBox.Show(string.Format(Language.Data.MiscDiscordPostErrMsg, webHook.Name),
                     Language.Data.MiscDiscordPostErrTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        
        public void SetSelectedAutoTasks(bool eap, bool eau)
        {
            Settings.EnableAutoParse = eap;
            Settings.EnableAutoUpload = eau;

            var settings = SettingsData.Load();
            settings.EnableAutoParse = eap;
            settings.EnableAutoUpload = eau;
            settings.Save();
        }

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
            List<ICachedLog> logs = new List<ICachedLog>();
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
            List<ICachedLog> logs = new List<ICachedLog>();
            foreach (var id in ids)
            {
                if (token.IsCancellationRequested) return null;
                var log = QuickCacheLog(id);
                if (log != null) logs.Add(log);
            }
            if (token.IsCancellationRequested) return null;
            return GetPreview(token, logs);
        }

        private LogPreview GetPreview(CancellationToken token, List<ICachedLog> logs)
        {
            if (logs.Count == 1)
            {
                var log = logs[0];
                var pData = log.PlayersNew?.OrderByDescending(p => p.DpsTargets);
                return new LogPreview(log, pData.Count() > 0 ? pData : null, 0 < log.DataVersion && log.DataVersion < CachedLog.CurrentDataVersion);
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

                //MAYBE performace improvement via summing and compare count (or not?)
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

                bool outDatedJson = logs.Any(log => 0 < log.DataVersion && log.DataVersion < CachedLog.CurrentDataVersion);

                if (token.IsCancellationRequested) return null;

                return new LogPreview(name, size, maxHP, date, true, totalDuration, Corrected, IsCM,
                        Success, HasHtml, true, "", HasLink, "", null, outDatedJson);
            }
            return new LogPreview("", 0, 0, DateTime.Now, false, new TimeSpan(0), CheckState.Indeterminate, CheckState.Indeterminate,
                CheckState.Indeterminate, CheckState.Indeterminate, false, null, CheckState.Indeterminate, null, null, false);
        }

        public ICachedLog CacheLog(int id)
        {
            var log = QuickCacheLog(id);
            if (!string.IsNullOrWhiteSpace(log.JsonPath) && ((log.PlayersNew?.Count ?? 0) == 0))
            {
                if (!File.Exists(log.JsonPath))
                    return log;
                var jsonStr = JsonHandling.ReadJsonFile(log.JsonPath);
                var simpleLogJson = new SimpleLogJson(jsonStr);
                log.ApplySimpleLog(simpleLogJson);
            }
            GC.Collect();
            LogCache.Add(log);
            return log;
        }

        public void UpdateFilePaths(ICachedLog log)
        {
            if (string.IsNullOrEmpty(log.EvtcPath) || !File.Exists(log.EvtcPath))
                log.EvtcPath = null;
            if (string.IsNullOrEmpty(log.HtmlPath) || !File.Exists(log.HtmlPath))
                log.HtmlPath = null;
            if (string.IsNullOrEmpty(log.JsonPath) || !File.Exists(log.JsonPath))
                log.JsonPath = null;
            LogDBConnector.Update(log.GetDBLog());
        }

        public void UpdateWhatsNew(string version)
        {
            Settings.WhatsNewShown = version;
            var settings = SettingsData.Load();
            settings.WhatsNewShown = version;
            settings.Save();

        }

        internal void UpdateRaidOrga(RaidSimple data, List<int> list, CancellationToken ct, Action<Delegate> invoker, IProgress<ProgressMessage> progress = null)
        {

            progress?.Report(new ProgressMessage(0, "Check RO+ Login"));
            if (data is RaidSimpleTemplate t)
            {
                //TODO localize error
                Action a = () => MessageBox.Show(t.DisplayName, "Invalid raid", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                invoker(a);
                return;
            }

            if (!CheckRaidOrgaSession(invoker, progress)) return;

            progress?.Report(new ProgressMessage(0.05, "Gathering RO+ data"));
            Raid raid = RaidOrgaPlusConnector.GetRaid(RaidOrgaPlusSession, data.TerminID, data.RaidID, ct, new Progress<ProgressMessage>((p) => progress?.Report(new ProgressMessage((p.Percent * 0.35) + 0.05, "Gathering RO+ data - " + p.Message))));
            if (ct.IsCancellationRequested) return;
            progress?.Report(new ProgressMessage(0.35, "Gathering local data"));
            var PercentPerLog = 0.4 / list.Count;
            List<ICachedLog> logs = new List<ICachedLog>();
            foreach ((var i, var id) in list.Enumerate())
            {
                var tmp = ProcessLog(PercentPerLog, i, id, progress, ct);
                if (ct.IsCancellationRequested) return;
                if (tmp != null)
                    logs.Add(tmp);
            };
            raid = RaidOrgaPlusDataWorker.UpdateRaid(raid, logs, invoker, new Progress<ProgressMessage>((p) => progress?.Report(new ProgressMessage((p.Percent * 0.1) + 0.8, "Processing data - " + p.Message))));

            if (ct.IsCancellationRequested) return;
            progress?.Report(new ProgressMessage(0.95, "Updating RO+"));
            try
            {
                RaidOrgaPlusConnector.SetRaid(RaidOrgaPlusSession, raid);
            }
            catch (Exception e)
            {
                Logger.Error("Exeption in LogUploaderLogic.UpdateRaidOrga when setting raid");
                Logger.LogException(e);
            }
            progress?.Report(new ProgressMessage(0.99, "Done"));
        }

        private bool CheckRaidOrgaSession(Action<Delegate> invoker, IProgress<ProgressMessage> progress)
        {
            progress?.Report(new ProgressMessage(0.01, "Check RO+ Session"));
            if (!RaidOrgaPlusSession.Valid)
            {
                progress?.Report(new ProgressMessage(0.01, "Reconnect RO+ Session"));
                RaidOrgaPlusSession = RaidOrgaPlusConnector.Connect(Settings);
            }
            if (RaidOrgaPlusSession == null)
            {
                Action a = () => MessageBox.Show(Language.Data.MiscRaidOrgaPlusLoginErr, "RO+ login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                invoker(a);
                return false;
            }
            if (!RaidOrgaPlusSession.Valid)
                return false;
            return true;
        }

        private ICachedLog ProcessLog(double PercentPerLog, int i, int id, IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            var log = QuickCacheLog(id);
            var percent = i * PercentPerLog;
            Console.WriteLine($"Gathering local data - Caching {log.BossName} {log.Date.TimeOfDay:hh':'mm}");
            progress?.Report(new ProgressMessage(percent + 0.4, $"Gathering local data - Caching {log.BossName} {log.Date.TimeOfDay:hh':'mm}"));
            log = CacheLog(id);
            if (ct.IsCancellationRequested) return null;
            if (log.DataVersion < RaidOrgaPlusDataWorker.MIN_DATA_VERSION)
            {
                percent += PercentPerLog / 2;
                Console.WriteLine($"Gathering local data - Updating {log.BossName} {log.Date.TimeOfDay:hh':'mm}");
                progress?.Report(new ProgressMessage(percent + 0.4, $"Gathering local data - Updating {log.BossName} {log.Date.TimeOfDay:hh':'mm}"));
                log = ReParseData(log) ?? log;
            }
            return log;
        }

        internal List<RaidSimple> GetRaidOrgaTermine(bool reconnect = false, IProgress<ProgressMessage> progress = null)
        {
            if (reconnect) LoadTermine(progress);
            return RaidOrgaPlusTermine;
        }

        private void LoadTermine(IProgress<ProgressMessage> progress = null)
        {
            RaidOrgaPlusTermine = new List<RaidSimple>();
            if (!Settings.RaidOrgaPlusAccoutSet)
            {
                RaidOrgaPlusTermine.Add(RaidSimple.GetNoAccount());
                Logger.Message("[LogUploaderLogic.LoadTermine] No RO+ Account");
                progress?.Report(new ProgressMessage(1, "No Account"));
                return;
            }
            progress?.Report(new ProgressMessage(0, "Login"));
            RaidOrgaPlusConnector = new RaidOrgaPlusConnector(Settings);
            RaidOrgaPlusSession = RaidOrgaPlusConnector.Connect(Settings);
            if (RaidOrgaPlusSession == null)
            {
                RaidOrgaPlusTermine.Add(RaidSimple.GetLogInFaild());
                Logger.Error("[LogUploaderLogic.LoadTermine] RO+ Login Faild");
                progress?.Report(new ProgressMessage(1, "Log in Failed"));
                return;
            }
            progress?.Report(new ProgressMessage(0.5, "Raids"));
            RaidOrgaPlusTermine = RaidOrgaPlusConnector.GetRaids(RaidOrgaPlusSession, new Progress<double>(p => progress?.Report(new ProgressMessage((0.4 * p) + 0.5, "Raids"))));
            if (RaidOrgaPlusTermine.Count == 0)
            {
                RaidOrgaPlusTermine.Add(RaidSimple.GetNoTermine());
                Logger.Message("[LogUploaderLogic.LoadTermine] No Raid appointments");
            }
            progress?.Report(new ProgressMessage(1, "Done"));
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
                    WatchDogTask.Wait();
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

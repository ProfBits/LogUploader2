using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LogUploader.RaidOrgaPlus.Data;
using LogUploader.Tools;

using Newtonsoft.Json;

namespace LogUploader.Tools.RaidOrgaPlus
{
    public class RaidOrgaPlusCache
    {
        private Dictionary<long, List<RaidOrgaPlusCacheItem>> Data { get; set; }
        public string CHACH_FILE_PATH {
            get {
                return $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\LogUploader\RaidOrgaPlusCache.dat";
            }
        }

        public RaidOrgaPlusCache()
        {
            LoadFile();
        }

        private void LoadFile()
        {
            if (!File.Exists(CHACH_FILE_PATH))
            {
                Data = new Dictionary<long, List<RaidOrgaPlusCacheItem>>();
                return;
            }
            var zipped = File.ReadAllBytes(CHACH_FILE_PATH);
            var json = ZipHelper.Unzip(zipped);
            Data = JsonConvert.DeserializeObject<Dictionary<long, List<RaidOrgaPlusCacheItem>>>(json);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Data);
            var zipped = ZipHelper.Zip(json);
            File.WriteAllBytes(CHACH_FILE_PATH, zipped);
        }

        public long Get(long raidID, string Account)
        {
            if (Data.ContainsKey(raidID))
                return Data[raidID].Where(e => e.AccountName == Account).FirstOrDefault()?.AccountID ?? -1;
            return -1;
        }

        public void Set(long raidId, string accountName, long raidOrgaAccId)
        {
            if (!Data.ContainsKey(raidId))
                Data.Add(raidId, new List<RaidOrgaPlusCacheItem>());
            var raid = Data[raidId];
            if (raid.Any(e => e.AccountName == accountName))
                raid.First(eBosses => eBosses.AccountName == accountName).AccountID = raidOrgaAccId;
            else
                raid.Add(new RaidOrgaPlusCacheItem(accountName, raidOrgaAccId));
        }
    }
}

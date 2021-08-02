using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using LogUploader.Localisation;

namespace LogUploader.Data.Repositories
{
    internal class BossRepository : EnemyRepository<Boss>, BossProvider
    {
        private readonly MultiKeyInValueDictionary<int, string, string, Boss> Data;

        public BossRepository()
        {
            Data = new MultiKeyInValueDictionary<int, string, string, Boss>(
                boss => boss.ID,
                boss => boss.FolderNameEN,
                boss => boss.FolderNameDE
                );
        }

        internal override IDictionary<int, Boss> BaseData { get => GetBaseData(); }

        private IDictionary<int, Boss> GetBaseData()
        {
            return Data.ToDictionary(e => e.Key.Item1, e => e.Value);
        }

        public override Boss Get(int id)
        {
            return base.Get(CorrectID(id));
        }

        private int CorrectID(int id)
        {
            switch (id)
            {
                /*Fake Ai's*/
                case 23255:
                case 23256:
                    return (int)eBosses.Ai;
                default:
                    return id;
            }
        }

        public Boss Get(eBosses boss) => Get((int)boss);

        public Boss GetByEiName(string eiName)
        {
            return Data.Select(e => e.Value).FirstOrDefault(e => e.EIName == eiName) ?? throw new KeyNotFoundException();
        }

        public Boss GetByFolderName(string folderName)
        {
            try
            {
                return GetByFolderNameEN(folderName);
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    return GetByFolderNameDE(folderName);
                }
                catch (KeyNotFoundException)
                {
                    return Get(eBosses.Unknown);
                }
            }
        }

        private Boss GetByFolderNameDE(string folderNameDE)
        {
            switch (folderNameDE)
            {
                case "Gebrochener König":
                case "Bezwungener König":
                    return Get(eBosses.BrokenKing);
                default:
                    return Data.Get(key3: folderNameDE);
            }
        }
        private Boss GetByFolderNameEN(string folderNameEN)
        {
            switch (folderNameEN)
            {
                default:
                    return Data.Get(key2: folderNameEN);
            }
        }

        public Boss GetByFolderName(string folderName, eLanguage lang)
        {
            try
            {
                switch (lang)
                {
                    case eLanguage.DE:
                        return GetByFolderNameDE(folderName);
                    case eLanguage.EN:
                    default:
                        return GetByFolderNameEN(folderName);
                }
            }
            catch (KeyNotFoundException)
            {
                return Get(eBosses.Unknown);
            }
        }

        public Boss GetByRaidOrgaPlusID(int ropID)
        {
            IEnumerable<Boss> hits = this.Where(b => b.RaidOrgaPlusID == ropID);
            int numberOfHits = hits.Count();
            switch (numberOfHits)
            {
                case 0: throw new KeyNotFoundException($"RaidOrgaPlusID = {ropID} not found");
                case 1: return hits.First();
                default:
                    throw new ArgumentException($"RaidOrgaPlusID = {ropID} is not unique", nameof(ropID));
            }
        }

        public override bool Exists(int id)
        {
            return base.Exists(CorrectID(id));
        }

        public override IEnumerator<Boss> GetEnumerator()
        {
            return Data.Select(b => b.Value).GetEnumerator();
        }

        internal override void Add(Boss enemy)
        {
            if (enemy is null) throw new ArgumentNullException(nameof(enemy), "Cannot add a null boss to the repository");
            //if (enemy.ID != CorrectID(enemy.ID)) throw new ArgumentOutOfRangeException(nameof(enemy), enemy.ID, "The ID seems to be a virtual/fake boss id. The boss cannot be added.");
            Data.Add(enemy);
        }
    }
}

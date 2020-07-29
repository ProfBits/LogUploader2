using LogUploader.JSONHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class Profession : NamedObject
    {
        /// <summary>
        /// dict&lt;nameEN, Prfession&gt;
        /// </summary>
        private static IReadOnlyDictionary<string, Profession> Professions = new Dictionary<string, Profession>();

        private Profession() : base("") { }

        private Profession(string nameEN, string nameDE, string iconPath, string emote, int raidOrgaPlusID) : base(nameEN, nameDE)
        {
            IconPath = iconPath;
            Icon = Image.FromFile(iconPath);
            Emote = emote;
            RaidOrgaPlusID = raidOrgaPlusID;
        }

        public Profession this[string name]
        {
            get
            {
                return Professions.Select(p => p.Value).Where(p => p.NameEN == name || p.NameDE == name).FirstOrDefault() ?? Professions["Unknown"];
            }
        }

        public Profession this[int roPlusID]
        {
            get
            {
                return Professions.Select(p => p.Value).Where(p => p.RaidOrgaPlusID == roPlusID).FirstOrDefault() ?? Professions["Unknown"];
            }
        }

        public static Profession Get(string name)
        {
            return new Profession()[name];
        }

        public static Profession Get(int roPlusID)
        {
            return new Profession()[roPlusID];
        }

        public static Profession Unknown { get => Get("Unknown"); }

        public string IconPath { get; }
        public string Emote { get; }
        public Image Icon { get; internal set; }
        public int RaidOrgaPlusID { get; }


        public static void Init(string path, IProgress<double> progress = null)
        {
            progress?.Report(0);
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var strData = Helpers.GP.ReadJsonFile(path);
            progress?.Report(25);
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(strData);
            progress?.Report(50);
            var profList = (Newtonsoft.Json.Linq.JArray)jsonData["Professions"];
            Professions = profList.Select(json =>
            {
                string nameEN = (string)json["NameEN"];
                string nameDE = (string)json["NameDE"];
                string iconPath = exePath + (string)json["IconPath"];
                string emote = (string)json["Emote"];
                int raidOrgaPlusID = (int) json["RaidOrgaPlusID"];

                return new Profession(nameEN, nameDE, iconPath, emote, raidOrgaPlusID);
            })
            .ToDictionary(prof => prof.NameEN);
            progress?.Report(1);
        }
    }
}

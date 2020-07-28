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

        private Profession(string nameEN, string nameDE, string iconPath, string emote) : base(nameEN, nameDE)
        {
            IconPath = iconPath;
            Icon = Image.FromFile(iconPath);
            Emote = emote;
        }

        public Profession this[string name]
        {
            get
            {
                return Professions.Select(p => p.Value).Where(p => p.NameEN == name || p.NameDE == name).FirstOrDefault() ?? Professions["Unknown"];
            }
        }

        public static Profession Get(string name)
        {
            return new Profession()[name];
        }

        public string IconPath { get; }
        public string Emote { get; }
        public Image Icon { get; internal set; }

        public static void Init(string path, IProgress<double> progress = null)
        {
            progress?.Report(0);
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var strData = Helpers.GP.ReadJsonFile(path);
            progress?.Report(25);
            var jsonData = new JSONHelper.JSONHelper().Desirealize(strData);
            progress?.Report(50);
            var profList = jsonData.GetTypedList<JSONObject>("Professions");
            Professions = profList.Select(json =>
            {
                string nameEN = json.GetTypedElement<string>("NameEN");
                string nameDE = json.GetTypedElement<string>("NameDE");
                string iconPath = exePath + json.GetTypedElement<string>("IconPath");
                string emote = json.GetTypedElement<string>("Emote");

                return new Profession(nameEN, nameDE, iconPath, emote);
            })
            .ToDictionary(prof => prof.NameEN);
            progress?.Report(1);
        }
    }
}

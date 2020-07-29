using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LogUploader
{
    public class CachedPlayer
    {
        public string AccountName { get; }
        public string CharakterName { get; }
        public Profession Class { get; }
        public Image ProfessionIcon { get => Class.Icon; }
        public byte SubGroup { get; }
        public int DPS { get; }
        public int PDPS { get; }
        public int CDPS { get; }

        public CachedPlayer(string accountName, string charakterName, Profession profession, byte subGroup, int dPS, int pDPS, int cDPS)
        {
            AccountName = accountName;
            CharakterName = charakterName;
            Class = profession;
            SubGroup = subGroup;
            DPS = dPS;
            PDPS = pDPS;
            CDPS = cDPS;
        }

        public CachedPlayer(string json) : this(JObject.Parse(json))
        { }

        public CachedPlayer(JObject data)
        {
            AccountName = (string)data["account"];
            CharakterName = (string)data["name"];
            Class = Profession.Get((string)data["profession"]);
            SubGroup = (byte)data["group"];

            try
            {
                DPS = (int)data["dpsAll"][0]["dps"];
            }
            catch (IndexOutOfRangeException)
            {
                DPS = 0;
            }
            try
            {
                PDPS = (int)data["dpsAll"][0]["powerDps"];
            }
            catch (IndexOutOfRangeException)
            {
                PDPS = 0;
            }
            try
            {
                CDPS = (int)data["dpsAll"][0]["condiDps"];
            }
            catch (IndexOutOfRangeException)
            {
                CDPS = 0;
            }
        }
    }
}

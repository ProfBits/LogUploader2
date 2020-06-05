using LogUploader.Data;
using LogUploader.JSONHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CachedPlayer(string accountName, string charakterName, Profession profession, byte subGroup, int dPS)
        {
            AccountName = accountName;
            CharakterName = charakterName;
            Class = profession;
            SubGroup = subGroup;
            DPS = dPS;
        }

        public CachedPlayer(JSONObject data)
        {
            AccountName = data.GetTypedElement<string>("account");
            CharakterName = data.GetTypedElement<string>("name");
            Class = Profession.Get(data.GetTypedElement<string>("profession"));
            SubGroup = (byte) data.GetTypedElement<double>("group");

            try
            {
                DPS = (int)data.GetTypedList<JSONObject>("dpsAll")[0].GetTypedElement<double>("dps");
            }
            catch (IndexOutOfRangeException)
            {
                DPS = 0;
            }
            
        }
    }
}

﻿using LogUploader.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LogUploader.Localisation;
using LogUploader.Tools.Settings;
using LogUploader.Data.Discord;

namespace LogUploader.Tools.Discord
{
    public partial class WebHookDB
    {
        [JsonIgnore]
        private readonly Dictionary<long, WebHook> Data = new Dictionary<long, WebHook>();
        [JsonProperty("WebHooks")]
        public List<WebHook> WebHooks { get => Data.Values.ToList(); }

        public WebHookDB(string RawData)
        {
            if (string.IsNullOrWhiteSpace(RawData))
                return;

            //string data = encrypted ? SettingsHelper.UnprotectString(RawData) : RawData;

            var root = JObject.Parse(RawData);
            var webhooks = (JArray)root["WebHooks"];
            
            foreach(JObject webhook in webhooks)
            {
                var webhookData = new WebHook(webhook);
                Data.Add(webhookData.ID, webhookData);
            }

        }

        [Obsolete("circel reference", true)]
        public string GetSaveString()
        {
            //return SettingsHelper.ProtectString(ToString());
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public WebHook this[string alias]
        {
            get
            {
                var hook = Data.Where(e => e.Value.Name == alias).FirstOrDefault().Value;
                if (hook == null)
                {
                    if (Data.Count == 0)
                    {
                        MessageBox.Show(Language.Data.MiscNoWebhookMsgText, Language.Data.MiscNoWebhookMsgTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return new WebHook(null, "<No WebHooks>", -1);
                    }
                    throw new ArgumentException($"No webhook with alias \"{alias}\" registerd!");
                }
                return hook;
            }
        }
        public WebHook this[long id]
        {
            get
            {
                if (Data.ContainsKey(id))
                    return Data[id];
                if (Data.Count == 0)
                {
                    MessageBox.Show(Language.Data.MiscNoWebhookMsgText, Language.Data.MiscNoWebhookMsgTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return new WebHook(null, "<No WebHooks>", -1);
                }
                throw new ArgumentException($"No webhook with id \"{id}\" registerd!");
            }
        }

        public List<WebHook> GetWebHooks()
        {
            if (Data.Count == 0)
                return new List<WebHook>() { new WebHook(null, "<No WebHooks>", -1) };
            return Data.Values.ToList();
        }

        public List<string> GetWebHookAliases()
        {
            if (Data.Count == 0)
                return new List<string>() { "<No WebHooks>" };
            return Data.Values.Select(wh => wh.Name).ToList();
        }

        public WebHook AddNewWebHook(string alias, string url)
        {
            var newWH = new WebHook(url, alias);
            Data.Add(newWH.ID, newWH);
            return newWH;
        }

        public WebHook RemoveWebHook(string alias)
        {
            if (Data.Count == 0)
                return null;
            var wh = this[alias];
            return RemoveWebHook(wh.ID);
            
        }

        public WebHook RemoveWebHook(long id)
        {
            if (Data.Count == 0)
                return null;
            var wh = this[id];
            if (Data.ContainsKey(wh.ID))
            {
                Data.Remove(wh.ID);
                return wh;
            }
            return null;
        }

        public WebHook GetNewWebHook()
        {
            var aliases = GetWebHookAliases();
            int i = 1;
            while (aliases.Contains($"New WebHook {i}"))
                i++;
            return AddNewWebHook($"New WebHook {i}", "");
        }

        public bool HasEntries() => Data.Count > 0;

        public int Count() => Data.Count;

        public bool IDExists(long id) => Data.ContainsKey(id);
    }
}
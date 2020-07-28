using LogUploader.Interfaces;
using LogUploader.JSONHelper;

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LogUploader.Data
{
    class SimpleLogJson : IJSONSerializeable
    {
        public SimpleLogJson(string data)
        {
            var parsed = JObject.Parse(data);
            if (parsed.ContainsKey("version"))
                Version = (int)parsed["version"];
            RecordedBy = (string)parsed["recordedBy"];
            Targets = ((JArray)parsed["targets"])
                .Select(targetData => new SimmpleTarget((JObject)targetData))
                .ToList();
            Players = ((JArray)parsed["players"])
                .Select(playerData => new SimplePlayer((JObject)playerData))
                .ToList();
        }

        public string RecordedBy { get; }
        public List<SimmpleTarget> Targets { get; }
        public List<SimplePlayer> Players { get; }
        public int Version { get; } = 1;

        public JSONObject GetJSONObject()
        {
            var json = new JSONObject();
            json.Values.Add("version", Version);
            json.Values.Add("recordedBy", RecordedBy);
            json.Values.Add("targets", Targets.Select(target => (object) target.GetJSONObject()).ToList());
            json.Values.Add("players", Players.Select(player => (object) player.GetJSONObject()).ToList());
            return json;
        }

    }
    public class SimplePlayer : IJSONSerializeable
    {
        public SimplePlayer(JSONObject data)
        {
            Account = data.GetTypedElement<string>("account");
            Name = data.GetTypedElement<string>("name");
            Profession = data.GetTypedElement<string>("profession");
            Group = (int)data.GetTypedElement<double>("group");
            try
            {
                DpsAll = (int)data.GetTypedElement<double>("dpsAll[0]/dps");
                DpsAllPower = (int)data.GetTypedElement<double>("dpsAll[0]/powerDps");
                DpsAllCondi = (int)data.GetTypedElement<double>("dpsAll[0]/condiDps");
            }
            catch (JSONException)
            {
                DpsAll = (int)data.GetTypedElement<double>("allDps");
                DpsAllPower = (int)data.GetTypedElement<double>("allPowerDps");
                DpsAllCondi = (int)data.GetTypedElement<double>("allCondiDps");
            }
        }

        public SimplePlayer(JObject data)
        {
            Account = (string)data["account"];
            Name = (string)data["name"];
            Profession = (string)data["profession"];
            Group = (int)data["group"];
            if (data["dpsAll"] is JArray)
            {
                DpsAll = (int)data["dpsAll"][0]["dps"];
                DpsAllPower = (int)data["dpsAll"][0]["powerDps"];
                DpsAllCondi = (int)data["dpsAll"][0]["condiDps"];
            }
            else
            {
                DpsAll = (int)data["allDps"];
                DpsAllPower = (int)data["allPowerDps"];
                DpsAllCondi = (int)data["allCondiDps"];
            }
        }

        public string Account { get; }
        public string Name { get; }
        public string Profession { get; }
        public int Group { get; }
        public int DpsAll { get; }
        public int DpsAllPower { get; }
        public int DpsAllCondi { get; }

        public JSONObject GetJSONObject()
        {
            var json = new JSONObject();
            json.Values.Add("account", Account);
            json.Values.Add("name", Name);
            json.Values.Add("profession", Profession);
            json.Values.Add("group", Group);
            json.Values.Add("allDps", DpsAll);
            json.Values.Add("allPowerDps", DpsAllPower);
            json.Values.Add("allCondiDps", DpsAllCondi);
            return json;
        }
    }
    public class SimmpleTarget : IJSONSerializeable
    {
        public SimmpleTarget(JSONObject data)
        {
            ID = (int)data.GetTypedElement<double>("id");
            TotalHealth = (int)data.GetTypedElement<double>("totalHealth");
            FinalHealth = (int)data.GetTypedElement<double>("finalHealth");
            FirstAware = (int)data.GetTypedElement<double>("firstAware");
            LastAware = (int)data.GetTypedElement<double>("lastAware");
        }

        public SimmpleTarget(JObject data)
        {
            ID = (int)data["id"];
            TotalHealth = (int)data["totalHealth"];
            FinalHealth = (int)data["finalHealth"];
            FirstAware = (int)data["firstAware"];
            LastAware = (int)data["lastAware"];
        }

        public int ID { get; }
        public int TotalHealth { get; }
        public int FinalHealth { get; }
        public int FirstAware { get; }
        public int LastAware { get; }

        public JSONObject GetJSONObject()
        {
            var json = new JSONObject();
            json.Values.Add("id", ID);
            json.Values.Add("totalHealth", TotalHealth);
            json.Values.Add("finalHealth", FinalHealth);
            json.Values.Add("firstAware", FirstAware);
            json.Values.Add("lastAware", LastAware);
            return json;
        }
    }
}

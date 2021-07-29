using System;
using System.Collections;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using LogUploader.Data.Repositories;
using LogUploader.Tools;
using System.Collections.Generic;

namespace LogUploader.Data
{
    public static class ProfessionDataBuilder
    {
        private const string ROOT_TAG = "Professions";
        private const string TAG_ID = "ID";
        private const string TAG_NAME_EN = "NameEN";
        private const string TAG_NAME_DE = "NameDE";
        private const string TAG_ICON_PATH = "IconPath";
        private const string TAG_EMOTE = "Emote";
        private const string TAG_ROP_ID = "RaidOrgaPlusID";
        private const string TAG_ROP_ABBREVIATION = "RaidOrgaPlusAbbreviation";

        public static void ParseJson(string json) => ParseJson(json, StaticData.ProfessionRepository);

        internal static void ParseJson(string json, ProfessionRepository professionRepo)
        {
            if (json is null) throw new ArgumentNullException(nameof(json), "The JSON string cannot be null");
            if (professionRepo is null) throw new ArgumentNullException(nameof(professionRepo), "ProfessionRepository cannot be null");

            JArray parsedData = ParseString(json);
            IEnumerable<Profession> professions = LoadProfessionsInToRepo(parsedData);
            StoreProfessions(professions, professionRepo);
        }

        private static void StoreProfessions(IEnumerable<Profession> professions, ProfessionRepository professionRepo)
        {
            List<eProfession> added = new List<eProfession>();

            try
            {
                foreach (var profession in professions)
                {
                    professionRepo.Add(profession);
                    added.Add(profession.ProfessionEnum);
                }
            }
            catch (ArgumentException ex)
            {
                foreach (var prof in added)
                {
                    professionRepo.Remove(prof);
                }
                throw ex;
            }
        }

        private static IEnumerable<Profession> LoadProfessionsInToRepo(JArray professionsData)
        {
            foreach (JObject professionData in professionsData)
            {
                yield return ParseProfession(professionData);
            }
        }

        private static JArray ParseString(string json)
        {
            try
            {
                JArray professions = (JArray)JObject.Parse(json)[ROOT_TAG];
                if (professions is null) throw new ArgumentException("Provided JSON does not contian required root object " + ROOT_TAG, nameof(json));
                return professions;
            }
            catch (JsonReaderException readerEx)
            {
                ArgumentException jsonEx = new ArgumentException("Provided JSON has invalid json format", nameof(json), readerEx);
                Tools.Logging.Logger.LogException(jsonEx);
                throw jsonEx;
            }
        }

        private static Profession ParseProfession(JObject professionData)
        {
            try
            {
                int id = (int)professionData[TAG_ID];
                string nameEN = (string)professionData[TAG_NAME_EN];
                string nameDE = (string)professionData[TAG_NAME_DE];
                string iconPath = (string)professionData[TAG_ICON_PATH];
                string emote = (string)professionData[TAG_EMOTE];
                int raidOrgaPlusID = (int)professionData[TAG_ROP_ID];
                string raidOrgaPlusAbbreviation = (string)professionData[TAG_ROP_ABBREVIATION];
                return new Profession(GP.IntToEnum<eProfession>(id), nameEN, nameDE, iconPath, emote, raidOrgaPlusID, raidOrgaPlusAbbreviation);
            }
            catch (JsonReaderException readerEx)
            {
                ArgumentException jsonEx = new ArgumentException($"Provided profession JSON object has invalid format or structure" +
                    $"at L{readerEx.LineNumber}:{readerEx.LinePosition}.\n" + professionData.ToString(), nameof(professionData), readerEx);
                Tools.Logging.Logger.LogException(jsonEx);
                throw jsonEx;
            }
        }
    }
}

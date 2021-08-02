using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LogUploader.Data.Repositories
{
    internal class ProfessionRepository : ProfessionProvider
    {
        protected MultiKeyInValueDictionary<eProfession, string, string, int, string, Profession> Professions =
            new MultiKeyInValueDictionary<eProfession, string, string, int, string, Profession>(
                p => p.ProfessionEnum,
                p => p.NameEN,
                p => p.NameDE,
                p => p.RaidOrgaPlusID,
                p => p.Abbreviation
                );

        public Profession Unknown { get => Get(eProfession.Unknown); }

        internal void Add(Profession prof)
        {
            Professions.Add(prof);
        }

        internal void Remove(eProfession prof)
        {
            Professions.Remove(prof);
        }

        public Profession Get(eProfession profession)
        {
            try
            {
                return Professions.Get(profession);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"eProfession.{profession} is not register in this profession repository", nameof(profession));
            }
        }

        //HACK, not a good Idea to use / index this
        public Profession Get(string name)
        {
            try
            {
                return Professions.Get(key2: name);
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    return Professions.Get(key3: name);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException($"Name {name} is not register in this profession repository", nameof(name));
                }
            }
        }

        public Profession Get(int raidOrgaPlusID)
        {
            try
            {
                return Professions.Get(raidOrgaPlusID);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"RaidOrgaPlusID {raidOrgaPlusID} is not register in this profession repository", nameof(raidOrgaPlusID));
            }
        }

        public Profession GetByAbbreviation(string abbreviation)
        {
            try
            {
                return Professions.Get(key5: abbreviation);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"Abbreviation {abbreviation} is not register in this profession repository", nameof(abbreviation));
            }
        }
    }
}

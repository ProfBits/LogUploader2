﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    public class Account
    {
        public long ID { get; }
        public string AccountName { get; }
        public string Name { get; }

        public Account(long iD, string accountName, string name)
        {
            ID = iD;
            AccountName = accountName;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Account a)
                return a.ID == ID;
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Account a, Account b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Account a, Account b)
        {
            return !a.Equals(b);
        }
    }
}

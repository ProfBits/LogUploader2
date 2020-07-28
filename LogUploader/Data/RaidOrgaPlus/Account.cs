using System;
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
        public string UIString { get => $"{AccountName} ({Name})"; }

        public Account(long iD, string accountName, string name)
        {
            ID = iD;
            switch (ID)
            {
                case 0:
                    Name = "";
                    AccountName = "";
                    break;
                case 1:
                    Name = "LFG";
                    AccountName = "";
                    break;
                default:
                    AccountName = accountName;
                    Name = name;
                    break;
            }
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
            return UIString;
        }

        public static bool operator ==(Account a, Account b)
        {
            return a?.Equals(b) ?? (object)b == null;
        }

        public static bool operator !=(Account a, Account b)
        {
            return !a?.Equals(b) ?? (object)b == null;
        }
    }
}

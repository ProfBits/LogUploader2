using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    class Session
    {
        public TimeSpan Timeout => new TimeSpan(24, 0, 0);
        public string Token { get; }
        public string UserAgent { get; } = "LogUploader";
        public DateTime Created { get; }
        public bool Valid => DateTime.Now.Subtract(Created) < Timeout;

        public Session(string token, string userAgent)
        {
            Token = token;
            UserAgent = userAgent;
            Created = DateTime.Now;
        }


        public override bool Equals(object obj)
        {
            if (obj is Session a)
                return a.Token == Token;
            return false;
        }

        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Session a, Session b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Session a, Session b)
        {
            return !a.Equals(b);
        }
    }
}

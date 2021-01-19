using LogUploader.Helper;
using LogUploader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public class NamedObject : INamedObject
    {
        public string NameEN { get; }
        public string NameDE { get; }
        public virtual string Name { get => GetName(Languages.Language.Current); }

        public NamedObject(string nameEN, string nameDE)
        {
            NameEN = nameEN;
            NameDE = nameDE;
        }

        public NamedObject(string name) : this(name, name)
        { }

        public NamedObject(INamedObject namedObject) : this(namedObject.NameEN, namedObject.NameDE)
        { }

        public virtual string GetName(eLanguage language)
        {
            switch (language)
            {
                case eLanguage.DE:
                    return NameDE;
                case eLanguage.EN:
                default:
                    return NameEN;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is NamedObject other)
            {
                return NameDE == other.NameDE && NameEN == other.NameEN;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return NameEN.GetHashCode() ^ NameDE.GetHashCode();
        }

        public static bool operator ==(NamedObject a, NamedObject b)
        {
            return GP.Compare(a, b);
        }

        public static bool operator !=(NamedObject a, NamedObject b)
        {
            return !(a == b);
        }
        
        public bool HasName(string name)
        {
            return NameEN == name
                || NameDE == name;
        }

        public Prefixed Prefix(string prefix) => new Prefixed(this, prefix);

        public class Prefixed : NamedObject
        {
            public string PrefixStr { get; set; }

            public Prefixed(INamedObject namedObject, string prefix) : base(namedObject)
            {
                PrefixStr = prefix;
            }

            public override string Name => PrefixStr + base.Name;
        }
    }
}

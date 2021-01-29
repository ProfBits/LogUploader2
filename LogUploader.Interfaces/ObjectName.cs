
using LogUploader.Localisation;

using System;

namespace LogUploader
{
    public class ObjectName : Attribute
    {
        public static Func<eLanguage> GetCurrentLanguage { private get; set; } = () => eLanguage.EN;

        public string NameDE { get; private set; }
        public string NameEN { get; private set; }

        public string Name
        {
            get
            {
                switch (GetCurrentLanguage())
                {
                    case eLanguage.DE:
                        return NameDE;
                    case eLanguage.EN:
                    default:
                        return NameEN;
                }
            }
        }

        public ObjectName(string nameEN, string nameDE)
        {
            NameDE = nameDE;
            NameEN = nameEN;
        }

        public ObjectName(string name)
        {
            NameDE = name;
            NameEN = name;
        }

        protected ObjectName() { }

        protected void ChangeBase(string nameEN, string nameDE)
        {
            NameDE = nameDE;
            NameEN = nameEN;
        }
    }
}

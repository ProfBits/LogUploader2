using Extensiones;

using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader
{
    public class CombBoxView : Attribute
    {
        public CombBoxView(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

    public class ObjectName : Attribute
    {
        public string NameDE { get; private set; }
        public string NameEN { get; private set; }

        public string Name
        {
            get
            {
                switch (Language.Current)
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

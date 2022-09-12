using LogUploader.Injection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Localization
{
    [Service(ServiceType.Singelton)]
    public interface ILocalisation
    {
        public ILocalStrings Strings { get; }
        public event LocalisationChangedHandler LocalisationChanged;
    }

    public interface ILocalStrings
    {
        public string ResolveStringOrDefault(string section, string key, string defaultValue);
    }

    public delegate void LocalisationChangedHandler(object sender, EventArgs args);
}

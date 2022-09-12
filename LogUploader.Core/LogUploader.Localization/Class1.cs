

using LogUploader.Injection;

using System.IO;

namespace LogUploader.Localization
{
    internal static class LocalisationFactory
    {
        public static ILocalisation Create()
        {
            return new Localisation(LocalStrings.LoadEmpty());
        }
    }

    internal class Localisation : ILocalisation
    {
        public ILocalStrings Strings { get; private set; }

        public Localisation(ILocalStrings strings)
        {
            Strings = strings;
        }
        
        private void UpdateLanguage(string path)
        {
            Strings = LocalStrings.LoadFromFile(path);

            OnLocalisationChanged();
        }

        public event LocalisationChangedHandler? LocalisationChanged;

        private void OnLocalisationChanged()
        {
            LocalisationChanged?.Invoke(this, new EventArgs());
        }
    }

    internal class LocalStrings : ILocalStrings
    {
        private IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _localizedStrings;

        public LocalStrings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> localizedStrings)
        {
            _localizedStrings = localizedStrings;
        }

        public string ResolveStringOrDefault(string section, string key, string defaultValue)
        {
            if (_localizedStrings.ContainsKey(section))
            {
                var dataSection = _localizedStrings[section];
                if (dataSection.ContainsKey(key))
                {
                    return dataSection[key];
                }
            }
            return defaultValue;
        }

        internal static LocalStrings LoadFromFile(string paht)
        {
            return new LocalStrings(new Dictionary<string, IReadOnlyDictionary<string, string>>());
        }
        internal static LocalStrings LoadEmpty()
        {
            return new LocalStrings(new Dictionary<string, IReadOnlyDictionary<string, string>>());
        }
    }

    public class Registrator : IServiceRegistrator
    {
        public async Task Load(IServiceCollection serviceCollection)
        {
            await Task.Run(() => serviceCollection.Add(LocalisationFactory.Create()));
        }
    }
}
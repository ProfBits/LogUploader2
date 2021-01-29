using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using LogUploader.Tools.Logger;

namespace LogUploader.Helper
{
    class SettingsProvider : System.Configuration.SettingsProvider
    {
        const string NAME = "name";
        const string SERIALIZE_AS = "serializeAs";
        const string CONFIG = "configuration";
        const string USER_SETTINGS = "userSettings";
        const string SETTING = "setting";
#if DEBUG
#warning Special conf for branch
        const string FILE_NAME = "user.debugRESTRUCTUR.config";
#else
        const string FILE_NAME = "user.config";
#endif

        /// <summary>
        /// Loads the file into memory.
        /// </summary>
        public SettingsProvider()
        {

            SettingsDictionary = new Dictionary<string, SettingStruct>();
        }

        static SettingsProvider()
        {
            var newPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\LogUploader\";
            var config = newPath + FILE_NAME;
            ConfigPathUser = config;
            if (File.Exists(config))
            {
                Logger.Message($"Loading existing config: \"{config}\"");
                return;
            }

            Logger.Warn($"Config does not exist: \"{config}\". Looking for other configs");
            TryCreateConfigFromExisting(newPath, config);
        }

        private static void TryCreateConfigFromExisting(string newPath, string config)
        {
            var oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Bits_Inc\";
            string[] oldFiles;
            if (Directory.Exists(oldPath))
                oldFiles = Directory.GetFiles(oldPath, "*.config", SearchOption.AllDirectories);
            else
                oldFiles = new string[] { };
            string[] newFiles;
            if (Directory.Exists(oldPath))
                newFiles = Directory.GetFiles(oldPath, "*.config", SearchOption.TopDirectoryOnly);
            else
                newFiles = new string[] { };
            var files = oldFiles.Concat(newFiles);

            Logger.Message($"Found {oldFiles.Length} old and {newFiles.Length} new configs.");
            if ((oldFiles.Length + newFiles.Length) > 0)
            {
                var oldConfig = files.Select(f => new FileInfo(f)).Aggregate((f1, f2) => f1.LastWriteTime > f2.LastWriteTime ? f1 : f2).FullName;

                Logger.Message($"Newest config is \"{oldConfig}\".");
                try
                {
                    Directory.CreateDirectory(newPath);
                    File.Copy(oldConfig, config);
                }
                catch (UnauthorizedAccessException e)
                {
                    Logger.Error("No Accsess");
                    Logger.LogException(e);
                }
                catch (DirectoryNotFoundException e)
                {
                    Logger.Error("Directory not found");
                    Logger.LogException(e);
                }
                catch (FileNotFoundException e)
                {
                    Logger.Error("No Accsess");
                    Logger.LogException(e);
                }
                catch (IOException e)
                {
                    Logger.Error("IO Error");
                    Logger.LogException(e);
                }
                catch (NotSupportedException e)
                {
                    Logger.Error("Not supported");
                    Logger.LogException(e);
                }
            }
        }

        /// <summary>
        /// Override.
        /// </summary>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
        }

        /// <summary>
        /// Override.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name;
            }
            set
            {
                //do nothing
            }
        }

        /// <summary>
        /// Must override this, this is the bit that matches up the designer properties to the dictionary values
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            //load the file
            if (!_loaded)
            {
                _loaded = true;
                LoadValuesFromFile();
            }

            //collection that will be returned.
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            //itterate thought the properties we get from the designer, checking to see if the setting is in the dictionary
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;

                //need the type of the value for the strong typing
                var t = Type.GetType(setting.PropertyType.FullName);

                if (SettingsDictionary.ContainsKey(setting.Name))
                {
                    value.SerializedValue = SettingsDictionary[setting.Name].value;
                    value.PropertyValue = Convert.ChangeType(SettingsDictionary[setting.Name].value, t);
                }
                else //use defaults in the case where there are no settings yet
                {
                    value.SerializedValue = setting.DefaultValue;
                    value.PropertyValue = Convert.ChangeType(setting.DefaultValue, t);
                }

                values.Add(value);
            }
            return values;
        }

        /// <summary>
        /// Must override this, this is the bit that does the saving to file.  Called when Settings.Save() is called
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            //grab the values from the collection parameter and update the values in our dictionary.
            foreach (SettingsPropertyValue value in collection)
            {
                var setting = new SettingStruct()
                {
                    value = (value.PropertyValue == null ? String.Empty : value.PropertyValue.ToString()),
                    name = value.Name,
                    serializeAs = value.Property.SerializeAs.ToString()
                };

                if (!SettingsDictionary.ContainsKey(value.Name))
                {
                    SettingsDictionary.Add(value.Name, setting);
                }
                else
                {
                    SettingsDictionary[value.Name] = setting;
                }
            }

            //now that our local dictionary is up-to-date, save it to disk.
            SaveValuesToFile();
        }

        /// <summary>
        /// Loads the values of the file into memory.
        /// </summary>
        private void LoadValuesFromFile()
        {
            if (!File.Exists(UserConfigPath))
            {
                //if the config file is not where it's supposed to be create a new one.
                CreateEmptyConfig();
            }

            //load the xml
            var configXml = XDocument.Load(UserConfigPath);

            //get all of the <setting name="..." serializeAs="..."> elements.
            var settingElements = configXml.Element(CONFIG).Element(USER_SETTINGS).Element(typeof(Properties.Settings).FullName).Elements(SETTING);

            //iterate through, adding them to the dictionary, (checking for nulls, xml no likey nulls)
            //using "String" as default serializeAs...just in case, no real good reason.
            foreach (var element in settingElements)
            {
                var newSetting = new SettingStruct()
                {
                    name = element.Attribute(NAME) == null ? String.Empty : element.Attribute(NAME).Value,
                    serializeAs = element.Attribute(SERIALIZE_AS) == null ? "String" : element.Attribute(SERIALIZE_AS).Value,
                    value = element.Value ?? String.Empty
                };
                SettingsDictionary.Add(element.Attribute(NAME).Value, newSetting);
            }
        }

        /// <summary>
        /// Creates an empty user.config file...looks like the one MS creates.  
        /// This could be overkill a simple key/value pairing would probably do.
        /// </summary>
        private void CreateEmptyConfig()
        {
            var doc = new XDocument();
            var declaration = new XDeclaration("1.0", "utf-8", "true");
            var config = new XElement(CONFIG);
            var userSettings = new XElement(USER_SETTINGS);
            var group = new XElement(typeof(Properties.Settings).FullName);
            userSettings.Add(group);
            config.Add(userSettings);
            doc.Add(config);
            doc.Declaration = declaration;
            doc.Save(UserConfigPath);
        }

        /// <summary>
        /// Saves the in memory dictionary to the user config file
        /// </summary>
        private void SaveValuesToFile()
        {
            //load the current xml from the file.
            var import = XDocument.Load(UserConfigPath);

            //get the settings group (e.g. <Company.Project.Desktop.Settings>)
            var settingsSection = import.Element(CONFIG).Element(USER_SETTINGS).Element(typeof(Properties.Settings).FullName);

            //iterate though the dictionary, either updating the value or adding the new setting.
            foreach (var entry in SettingsDictionary)
            {
                var setting = settingsSection.Elements().FirstOrDefault(e => e.Attribute(NAME).Value == entry.Key);
                if (setting == null) //this can happen if a new setting is added via the .settings designer.
                {
                    var newSetting = new XElement(SETTING);
                    newSetting.Add(new XAttribute(NAME, entry.Value.name));
                    newSetting.Add(new XAttribute(SERIALIZE_AS, entry.Value.serializeAs));
                    newSetting.Value = (entry.Value.value ?? String.Empty);
                    settingsSection.Add(newSetting);
                }
                else //update the value if it exists.
                {
                    setting.Value = (entry.Value.value ?? String.Empty);
                }
            }
            import.Save(UserConfigPath);
        }

        /// <summary>
        /// The setting key this is returning must set before the settings are used.
        /// e.g. <c>Properties.Settings.Default.SettingsKey = @"C:\temp\user.config";</c>
        /// </summary>
        private string UserConfigPath { get => ConfigPathUser; }

        private static string ConfigPathUser { get; set; }

        /// <summary>
        /// In memory storage of the settings values
        /// </summary>
        private Dictionary<string, SettingStruct> SettingsDictionary { get; set; }

        /// <summary>
        /// Helper struct.
        /// </summary>
        internal struct SettingStruct
        {
            internal string name;
            internal string serializeAs;
            internal string value;
        }

        bool _loaded = false;
    }
}

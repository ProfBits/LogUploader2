

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord
{
    internal class Field
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static Field()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        public const int NAME_MAX_LENGHT = 256;
        private string name = "";
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Name
        {
            get => name;
            set => name = value.Length > NAME_MAX_LENGHT ? value.Substring(0, NAME_MAX_LENGHT) : value;
        }

        public const int VAL_MAX_LENGHT = 1024;
        private string val = "";
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Value
        {
            get => val;
            set => val = value.Length > VAL_MAX_LENGHT ? value.Substring(0, VAL_MAX_LENGHT) : value;
        }
        [JsonProperty("inline")]
        public bool Inline { get; set; } = false;

        public Field(string name, string value, bool inline)
        {
            Name = name;
            Value = value;
            Inline = inline;
        }

        public bool ShouldSerializeInline()
        {
            return !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Value);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}

using System.Collections.Generic;
using System.Drawing;


using Newtonsoft.Json;
using System.ComponentModel;

namespace LogUploader.Tools.Discord
{
    internal class Embed : IEmbed
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static Embed()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
        }

        [JsonIgnore]
        public const int MAX_LENGHT = 6000;
        [JsonProperty("author", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public IAuthor Author { get; set; } = null;

        [JsonIgnore]
        public const int TITLE_MAX_LENGHT = 256;
        private string title = "";
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Title
        {
            get => title;
            set => title = value.Length > TITLE_MAX_LENGHT ? value.Substring(0, TITLE_MAX_LENGHT) : value;
        }
        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string URL { get; set; } = "";

        [JsonIgnore]
        public const int DESCRIPTIONE_MAX_LENGHT = 2048;
        private string descriptione = "";
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Descriptione
        {
            get => descriptione;
            set => descriptione = value.Length > DESCRIPTIONE_MAX_LENGHT ? value.Substring(0, DESCRIPTIONE_MAX_LENGHT) : value;
        }

        [JsonIgnore]
        public Color Color { get; set; } = Color.FromArgb(255, 0, 254);
        [JsonProperty("color", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue((20 << 16) + (20 << 8) + 20)]
        public int JsonColor { get => (Color.R << 16) + (Color.G << 8) + Color.B; set => Color.FromArgb((value & (0b1111_1111 << 16)) >> 16, (value & (0b1111_1111 << 8)) >> 8, value & 0b1111_1111); }
        /// <summary>
        /// Max count of 25
        /// </summary>
        [JsonProperty("fields", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<IField> Fields { get; set; } = new List<IField>();
        [JsonIgnore]
        public const int MAX_FIELDS = 25;

        [JsonProperty("thumbnail", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public IThumbmail Thumbmail { get; set; } = null;

        [JsonProperty("image", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public IImage Image { get; set; } = null;

        [JsonProperty("footer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public IFooter Footer { get; set; } = null;

        public Embed(IAuthor author, string title, Color color, List<IField> fields, IThumbmail thumbmail)
        {
            Author = author;
            Title = title;
            Color = color;
            Fields = fields;
            Thumbmail = thumbmail;
        }

        public Embed(IAuthor author, string title, string uRL, string descriptione, Color color, List<IField> fields, IThumbmail thumbmail, Image image, Footer footer) :
            this(author, title, color, fields, thumbmail)
        {
            URL = uRL;
            Descriptione = descriptione;
            Image = image;
            Footer = footer;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }
    }
}

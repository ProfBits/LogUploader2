using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.JSONHelper;
using LogUploader.Interfaces;

using Newtonsoft.Json;
using System.ComponentModel;

namespace LogUploader.Data
{
    public partial class WebHookData
    {


        public WebHookData(string username, string avaturURL, string content, List<Embed> embeds) : this(embeds)
        {
            Username = username;
            AvaturURL = avaturURL;
            Content = content;
        }

        public WebHookData(List<Embed> embeds)
        {
            Embeds.AddRange(embeds);
        }

        public WebHookData(Embed embed)
        {
            Embeds.Add(embed);
        }

        public WebHookData()
        {
        }

        [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Username { get; set; } = "LogUploader";
        [JsonProperty("avatar_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string AvaturURL { get; set; } = @"https://i.imgur.com/o0QwGPV.png";
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Content { get; set; } = "";
        [JsonProperty("embeds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Embed> Embeds { get; set; } = new List<Embed>();
        [JsonIgnore]
        public const int MAX_Embeds = 10;
        
        public bool ShouldSerializeEmbeds()
        {
            return Embeds.Count > 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public class Embed
        {
            [JsonIgnore]
            public const int MAX_LENGHT = 6000;
            [JsonProperty("author", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue(null)]
            public Author Author { get; set; } = null;

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
            public List<Field> Fields { get; set; } = new List<Field>();
            [JsonIgnore]
            public const int MAX_FIELDS = 25;

            [JsonProperty("thumbnail", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue(null)]
            public Thumbmail Thumbmail { get; set; } = null;

            [JsonProperty("image", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue(null)]
            public Image Image { get; set; } = null;

            [JsonProperty("footer", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue(null)]
            public Footer Footer { get; set; } = null;

            public Embed(Author author, string title, Color color, List<Field> fields, Thumbmail thumbmail)
            {
                Author = author;
                Title = title;
                Color = color;
                Fields = fields;
                Thumbmail = thumbmail;
            }

            public Embed(Author author, string title, string uRL, string descriptione, Color color, List<Field> fields, Thumbmail thumbmail, Image image, Footer footer) :
                this(author, title, color, fields, thumbmail)
            {
                URL = uRL;
                Descriptione = descriptione;
                Image = image;
                Footer = footer;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Author
        {
            public const int NAME_MAX_LENGHT = 256;
            private string name = "";
            [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string Name
            {
                get => name;
                set => name = value.Length > NAME_MAX_LENGHT ? value.Substring(0, NAME_MAX_LENGHT) : value;
            }

            [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string URL { get; set; } = "";

            [JsonProperty("icon_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string IconURL { get; set; } = "";

            public Author(string name, string uRL, string iconURL)
            {
                Name = name;
                URL = uRL;
                IconURL = iconURL;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Field
        {
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
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Thumbmail
        {
            [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            string URL { get; set; } = "";

            public Thumbmail(string uRL)
            {
                URL = uRL;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Image
        {
            [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string URL { get; set; } = "";

            public Image(string uRL)
            {
                URL = uRL;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Footer
        {
            [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string Text { get; set; } = "";

            [JsonProperty("icon_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
            [DefaultValue("")]
            public string IconURL { get; set; } = "";

            public Footer(string text, string iconURL)
            {
                Text = text;
                IconURL = iconURL;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}

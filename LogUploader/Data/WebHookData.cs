using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.JSONHelper;
using LogUploader.Interfaces;

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

        public string Username { get; set; } = "LogUploader";
        public string AvaturURL { get; set; } = @"https://i.imgur.com/o0QwGPV.png";
        public string Content { get; set; } = "";
        public List<Embed> Embeds { get; set; } = new List<Embed>();
        public const int MAX_Embeds = 10;

        public override string ToString()
        {
            var res = new JSONObject();
            if (Username != "")
            res.Values.Add("username", Username);
            if (AvaturURL != "")
                res.Values.Add("avatar_url", AvaturURL);
            if (Content != "")
                res.Values.Add("content", Content);
            if (Embeds.Count > 0)
                res.Values.Add("embeds", Embeds.Select(embed => (object)embed.GetJSONObject()).ToList());
            return res.ToString();
        }

        public class Embed : IJSONSerializeable
        {
            public const int MAX_LENGHT = 6000;
            public Author Author { get; set; } = null;

            public const int TITLE_MAX_LENGHT = 256;
            private string title = "";
            public string Title
            {
                get => title;
                set => title = value.Length > TITLE_MAX_LENGHT ? value.Substring(0, TITLE_MAX_LENGHT) : value;
            }
            public string URL { get; set; } = "";

            public const int DESCRIPTIONE_MAX_LENGHT = 2048;
            private string descriptione = "";
            public string Descriptione
            {
                get => descriptione;
                set => descriptione = value.Length > DESCRIPTIONE_MAX_LENGHT ? value.Substring(0, DESCRIPTIONE_MAX_LENGHT) : value;
            }

            public Color Color { get; set; } = Color.FromArgb(255, 0, 254);
            /// <summary>
            /// Max count of 25
            /// </summary>
            public List<Field> Fields { get; set; } = new List<Field>();
            public const int MAX_FIELDS = 25;
            public Thumbmail Thumbmail { get; set; } = null;
            public Image Image { get; set; } = null;
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

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (Author != null)
                    res.Values.Add("author", Author.GetJSONObject());
                if (Title != "")
                    res.Values.Add("title", Title);
                if (URL != "")
                    res.Values.Add("url", URL);
                if (Descriptione != "")
                    res.Values.Add("description", Descriptione);
                if (Color !=  Color.FromArgb(20, 20, 20))
                    res.Values.Add("color", (Color.R << 16) + (Color.G << 8) + Color.B);
                if (Fields.Count > 0)
                {
                    res.Values.Add("fields", Fields.Select(field => (object)field.GetJSONObject()).ToList());
                }
                if (Thumbmail != null)
                    res.Values.Add("thumbnail", Thumbmail.GetJSONObject());
                if (Image != null)
                    res.Values.Add("image", Image.GetJSONObject());
                if (Footer != null)
                    res.Values.Add("footer", Footer.GetJSONObject());
                return res;
            }
        }

        public class Author : IJSONSerializeable
        {
            public const int NAME_MAX_LENGHT = 256;
            private string name = "";
            public string Name
            {
                get => name;
                set => name = value.Length > NAME_MAX_LENGHT ? value.Substring(0, NAME_MAX_LENGHT) : value;
            }
            public string URL { get; set; } = "";
            public string IconURL { get; set; } = "";

            public Author(string name, string uRL, string iconURL)
            {
                Name = name;
                URL = uRL;
                IconURL = iconURL;
            }

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (Name != "")
                    res.Values.Add("name", Name);
                if (URL != "")
                    res.Values.Add("url", URL);
                if (IconURL != "")
                    res.Values.Add("icon_url", IconURL);
                return res;
            }
        }

        public class Field : IJSONSerializeable
        {
            public const int NAME_MAX_LENGHT = 256;
            private string name = "";
            public string Name
            {
                get => name;
                set => name = value.Length > NAME_MAX_LENGHT ? value.Substring(0, NAME_MAX_LENGHT) : value;
            }

            public const int VAL_MAX_LENGHT = 1024;
            private string val = "";
            public string Value
            {
                get => val;
                set => val = value.Length > VAL_MAX_LENGHT ? value.Substring(0, VAL_MAX_LENGHT) : value;
            }
            public bool Inline { get; set; } = false;

            public Field(string name, string value, bool inline)
            {
                Name = name;
                Value = value;
                Inline = inline;
            }

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (Name != "")
                    res.Values.Add("name", Name);
                if (Value != "")
                    res.Values.Add("value", Value);
                if (Name != "" || Value != "")
                    res.Values.Add("inline", Inline);
                return res;
            }
        }

        public class Thumbmail : IJSONSerializeable
        {
            string URL { get; set; } = "";

            public Thumbmail(string uRL)
            {
                URL = uRL;
            }

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (URL != "")
                    res.Values.Add("url", URL);
                return res;
            }
        }

        public class Image : IJSONSerializeable
        {
            public string URL { get; set; } = "";

            public Image(string uRL)
            {
                URL = uRL;
            }

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (URL != "")
                    res.Values.Add("url", URL);
                return res;
            }
        }

        public class Footer : IJSONSerializeable
        {
            public string Text { get; set; } = "";
            public string IconURL { get; set; } = "";

            public Footer(string text, string iconURL)
            {
                Text = text;
                IconURL = iconURL;
            }

            public JSONObject GetJSONObject()
            {
                var res = new JSONObject();
                if (Text != "")
                    res.Values.Add("text", Text);
                if (IconURL != "")
                    res.Values.Add("icon_url", IconURL);
                return res;
            }
        }
    }
}

using LogUploader.Data;
using LogUploader.Data.GameAreas;
using LogUploader.Data.Settings;
using LogUploader.Helpers;
using LogUploader.Interfaces;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper.DiscordPostGen
{
    public abstract class DiscordPostGenerator : IDiscordPostGen
    {
        public static IWebHookSettings Settings { protected get; set; }

        public virtual List<WebHookData> Generate(IEnumerable<CachedLog> logs, string userName, string avatarURL)
        {
            var parsedLogs = logs.Select(log => new ParsedData(log, GenerateField(log))).Where(log => log.Field != null);
            var groupedLogs = GroupFields(parsedLogs);
            var embeds = GetEmbeds(groupedLogs);
            var posts = GetPosts(embeds, userName, avatarURL);
            return posts.ToList();
        }

        protected abstract WebHookData.Field GenerateField(CachedLog log);

        protected abstract KeyValueList<Grouping, IEnumerable<ParsedData>> GroupFields(IEnumerable<ParsedData> data);

        protected virtual IEnumerable<WebHookData.Embed> GetEmbeds(IEnumerable<KeyValuePair<Grouping, IEnumerable<ParsedData>>> groupedData)
        {
            List<WebHookData.Embed> embeds = new List<WebHookData.Embed>();

            foreach (var group in groupedData)
            {
                var grouping = group.Key;
                var values = group.Value;

                var current = GetEmbedFrame(grouping, values);
                var color = GetColor(values.Select(data => data.Log));
                current.Color = color;

                foreach (var data in values)
                {
                    current.Fields.Add(data.Field);
                    var currentLenght = current.ToString().Length;
                    if (current.Fields.Count >= WebHookData.Embed.MAX_FIELDS)
                    {
                        if (currentLenght > WebHookData.Embed.MAX_LENGHT)
                            current.Fields.RemoveAt(current.Fields.Count - 1);
                        embeds.Add(current);
                        current = new WebHookData.Embed(null, "", color, new List<WebHookData.Field>(), null);
                        if (currentLenght > WebHookData.Embed.MAX_LENGHT)
                            current.Fields.Add(data.Field);
                    }
                }

                embeds.Add(current);
            }
            return embeds;
        }

        protected virtual WebHookData.Embed GetEmbedFrame(Grouping grouping, IEnumerable<ParsedData> values)
        {
            var dates = values.Select(log => log.Log.Date.Date).Distinct().ToList();
            dates.Sort();
            var authorName = GetDateString(dates.First());
            if (dates.Count > 1)
                authorName += " - " + GetDateString(dates.Last());
            var author = new WebHookData.Author(authorName, "", "");
            var thumbmail = new WebHookData.Thumbmail(grouping.AvatarURL);
            var name = grouping.Name;
            if (!string.IsNullOrWhiteSpace(grouping.PostFix))
                name += " " + grouping.PostFix;


            var current = new WebHookData.Embed(author, name, ColorDefault, new List<WebHookData.Field>(), thumbmail);
            return current;
        }

        protected virtual IEnumerable<WebHookData> GetPosts(IEnumerable<WebHookData.Embed> embeds, string userName, string avatarURL)
        {
            var posts = new List<WebHookData>();

            if (embeds.Count() == 0)
                return posts;

            var currentPost = GetDefaultPostHeader(userName, avatarURL);
            foreach (var embed in embeds)
            {
                if (currentPost.Embeds.Count >= WebHookData.MAX_Embeds)
                {
                    posts.Add(currentPost);
                    currentPost = GetDefaultPostHeader(userName, avatarURL);
                }
                currentPost.Embeds.Add(embed);
            }
            posts.Add(currentPost);
            return posts;
        }

        protected WebHookData GetDefaultPostHeader(string userName, string avatarURL)
        {
            const string Username = "LogUploader";
            var post = new WebHookData();
            post.Username = Settings.NameAsDiscordUser ? userName : Username;
            post.AvaturURL = avatarURL;
            post.Content = "";
            return post;
        }

        protected abstract Color GetColor(IEnumerable<CachedLog> logs);

        internal static IDiscordPostGen Get(eDiscordPostFormat format)
        {
            switch (format)
            {
                case eDiscordPostFormat.PerBoss:
                    return new PerBossGenerator();
                case eDiscordPostFormat.PerTryDetaild:
                    return new DetaildGenerator();
                case eDiscordPostFormat.PerAreaEmotes:
                    return new PerWingWithEmotes();
                case eDiscordPostFormat.CompactWithEmotes:
                    return new CompactWithEmotesGenerator();
                case eDiscordPostFormat.CompactWithClasses:
                    return new CompactWithClasesGenerator();
                case eDiscordPostFormat.PerAreaClasses:
                    return new PerWingWithClassesGenerator();
                case eDiscordPostFormat.PerArea:
                default:
                    return new PerWingGen();
            }
        }

        protected string GetDateString(DateTime date)
        {
            return date.ToString(Language.Current == eLanguage.DE ? @"dd\.MM\.yyyy" : @"MM\-dd\-yyyy");
        }

        protected string GetTimeString(DateTime date)
        {
            return date.TimeOfDay.ToString(@"HH\:mm");
        }

        protected static readonly Color ColorSucc = Color.FromArgb(0, 255, 0);
        protected static readonly Color ColorFail = Color.Red;
        protected static readonly Color ColorMixed = Color.Yellow;
        protected static readonly Color ColorDefault = Color.White;

        protected struct ParsedData
        {
            public CachedLog Log { get; }
            public WebHookData.Field Field { get; }

            public ParsedData(CachedLog log, WebHookData.Field field)
            {
                Log = log;
                Field = field;
            }
        }

        protected class Grouping : NamedObject, IAvatar
        {
            private static int ID = 0;
            private readonly int id;
            public string AvatarURL { get; }

            public string PostFix { get; set; } = "";

            public Grouping(string avatarURL, NamedObject name, string postFix = "") : base(name)
            {
                AvatarURL = avatarURL;
                id = ID++;
                PostFix = postFix;
            }

            public Grouping(IAvatar avatar, NamedObject name, string postFix = "") : this(avatar.AvatarURL, name, postFix)
            { }

            public Grouping(Boss boss, string postFix = "") : this(boss, boss, postFix)
            { }

            public Grouping(GameArea area, string postFix = "") : this(area, area, postFix)
            { }

            // override object.Equals
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                return base.Equals(obj) && AvatarURL == ((Grouping) obj).AvatarURL;
            }

            public bool Equals(Boss boss)
            {
                return base.Equals((NamedObject)boss) && AvatarURL == boss.AvatarURL;
            }

            public bool Equals(GameArea area)
            {
                return base.Equals((NamedObject)area) && AvatarURL == area.AvatarURL;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                return AvatarURL.GetHashCode() ^ base.GetHashCode();
            }

            public string getNameWithPostFix(eLanguage language)
            {
                if (string.IsNullOrWhiteSpace(PostFix))
                    return base.getName(language);
                return base.getName(language) + " " + PostFix;
            }
        }
    }
}

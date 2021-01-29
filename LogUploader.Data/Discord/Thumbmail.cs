

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public partial class WebHookData
    {
        public class Thumbmail : IThumbmail
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
    }
}

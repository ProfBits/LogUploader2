

using Newtonsoft.Json;

using System.ComponentModel;

namespace LogUploader.Tools.Discord.DiscordPostGen
{
    public partial class WebHookData
    {
        public class Image : IImage
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
    }
}

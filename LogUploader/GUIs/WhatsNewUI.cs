using LogUploader.Data.Settings;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class WhatsNewUI : Form
    {
        private const string BASE_ADDRESS = "https://api.github.com/repos/ProfBits/LogUploader2/releases/tags/";
        private const string HTML_PART_A = @"<html>
<header>
<style>
body {
font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Helvetica,Arial,sans-serif,Apple Color Emoji,Segoe UI Emoji;
color: #24292e;
word-wrap: break-word;
margin-bottom: 16px;
}
h1   {
margin-top: 0 !important;
font-size: 2em;
padding-bottom: .3em;
border-bottom: 1px solid #eaecef;
margin-top: 24px;
font-weight: 600;
line-height: 1.25;
}
h2 {
font-size: 1.5em;
padding-bottom: .3em;
border-bottom: 1px solid #eaecef;
margin-top: 24px;
font-weight: 600;
line-height: 1.25;
}
ul {
padding-left: 1em;
margin-top: 0;
font-size: 16px;
line-height: 1.5;
}
</style>
</header>
<body>";
        private const string HTML_PART_B = @"</body>
</html>";

        private const string HTML_LOAD_A = @"
<html>
    <head>
        <style>
            div {
                    position: absolute;
                    top: 50%;
                    margin-top: -100px;
                    left: 50%;
                    margin-left: -100px;
            }
        </style>
    </head>
    <body>
        <div>
            <img src=""";

        private const string HTML_LOAD_B = @"\images\loading.gif"" />
        </div>
    </body>
</html>";

        private readonly IProxySettings settings;
        private readonly string version;

        public WhatsNewUI(IProxySettings settings, string version)
        {
            this.settings = settings;
            this.version = version;

            InitializeComponent();
            ApplyLanguage(Language.Data);
            var html = HTML_LOAD_A + Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + HTML_LOAD_B;
            webBrowser1.DocumentText = html;
        }

        private void ApplyLanguage(ILanguage lang)
        {
            this.Text = lang.NewTitle + version;
            lblTitle.Text = lang.NewHeading;
            btnClose.Text = lang.NewClose;
        }

        private async Task LoadData(IProxySettings settings, string version)
        {
            await Task.Delay(500);
            var wc = Helper.WebHelper.GetWebClient(settings);
            wc.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "LogUploader");
            string res;
            try
            {
                res = wc.DownloadString(BASE_ADDRESS + version);
            }
            catch (System.Net.WebException)
            {
                Close();
                return;
            }
            var data = Newtonsoft.Json.Linq.JObject.Parse(res);
            var patchnotes = (string)data["body"];
            var htmlNotes = CommonMark.CommonMarkConverter.Convert(patchnotes);
            var html = HTML_PART_A + htmlNotes + HTML_PART_B;
            webBrowser1.DocumentText = html;
        }

        private void WhatsNewUI_Shown(object sender, EventArgs e)
        {
            _ = LoadData(settings, version);
        }

        private void BtnDismiss_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

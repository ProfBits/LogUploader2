using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Data;
using Extensiones;
using LogUploader.Localisation;
using LogUploader.Data.Discord;
using LogUploader.Tools.Discord;

namespace LogUploader.GUIs
{
    public partial class WebHookConfig : UserControl
    {
        private readonly WebHook WebHook;

        public WebHookConfig()
        {
            InitializeComponent();
            ApplyLanguage(Language.Data);
        }

        public WebHookConfig(WebHook webHook)
        {
            WebHook = webHook;
            InitializeComponent();
            ApplyLanguage(Language.Data);

            cmbFormat.ValueMember = "val";
            cmbFormat.DisplayMember = "name";
            cmbFormat.DataSource = 
                Helper.EnumHelper.GetValues<eDiscordPostFormat>()
                .Select(f => new { val = f, name = f.GetAttribute<ObjectName>().Name })
                .ToList();

            txtLink.Text = webHook.URL;
            txtName.Text = webHook.Name;
            txtAvatar.Text = webHook.AvatarURL;
            cmbFormat.SelectedValue = webHook.Format;
        }

        private void ApplyLanguage(ILanguage lang)
        {
            lblNameDes.Text = lang.ConfigDiscordWebHookName;
            lblLinkDes.Text = lang.ConfigDiscordWebHookLink;
            lblFormatDes.Text = lang.ConfigDiscordWebHookFormat;
            lblAvatarDes.Text = lang.ConfigDiscordWebHookAvatar;
            btnDelete.Text = lang.ConfigDiscordWebHookDelete;
        }

        public delegate void WebHookDeleteEventHandler(object sender, WebHookDeleteEventArgs e);
        public event WebHookDeleteEventHandler WebHookDeleted;

        private void OnWebHookDeleted(WebHookDeleteEventArgs e)
        {
            WebHookDeleted?.Invoke(this, e);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            OnWebHookDeleted(new WebHookDeleteEventArgs(WebHook.ID));
        }

        public WebHook SaveChanges()
        {
            WebHook.Name = txtName.Text;
            WebHook.URL = txtLink.Text;
            WebHook.Format = (eDiscordPostFormat)cmbFormat.SelectedValue;
            WebHook.AvatarURL = txtAvatar.Text;

            return WebHook;
        }
    }
}

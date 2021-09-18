using LogUploader.Data.Licenses;
using LogUploader.Localisation;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class SoftwareUsedUI : Form
    {
        public SoftwareUsedUI()
        {
            InitializeComponent();
            ApplyLanguage(Language.Data);
            InitLizenses();
        }

        private void InitLizenses()
        {
            Dapper.License = new DapperLicense();
            SQLiteCore.License = new SQLiteCoreLicense();
            WindowsAPICodePackCore.License = new WindowsAPICodePack_CoreLicense();
            WindowsAPICodePackShell.License = new WindowsAPICodePack_ShellLicense();
            MakeSFX.License = new MakeSFXLicense();
            CommonMark.License = new CommonMarkLicense();
            NewtonsoftJson.License = new NewtonsoftJsonLicense();
            PropMapper.License = new PropMapperLicense();
            MicrosoftBuild.License = new MicrosoftBuildLicense();
            MicrosoftBuildUtilitiesCore.License = new MicrosoftBuildUtilitiesCoreLicense();
            SystemResourcesExtensions.License = new SystemResourcesExtensionsLicense();
        }

        private void ApplyLanguage(ILanguage lang)
        {
            Text = lang.LicensesTitle;
            bntClose.Text = lang.LicensesClose;
        }

        private void BntClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

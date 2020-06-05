using LogUploader.Data.Licenses;
using LogUploader.Languages;
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
    public partial class SoftwareLicenseUI : Form
    {
        public SoftwareLicenseUI()
        {
            InitializeComponent();
        }

        public SoftwareLicenseUI(ISoftwareLicense license)
        {
            InitializeComponent();
            ApplyLanguage(Language.Data);

            Text = license.Product + " License";

            lblLizense.Text = license.Type;
            lblSoftware.Text = license.Product;
            lblOwner.Text = license.Owner;

            rtbLicenseText.Text = license.Text;
        }

        private void ApplyLanguage(ILanguage lang)
        {
            Text = lang.LicenseTitle;
            lblFor.Text = lang.LicenseFor;
            lblBy.Text = lang.LicenseBy;
        }
    }
}

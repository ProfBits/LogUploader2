
using LogUploader.RaidOrgaPlus.Data;
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

namespace LogUploader.Tools.RaidOrgaPlus.GUI
{
    public partial class CorrectPlayerUI : Form
    {
        private readonly List<PlayerToCorrect> ShowenPlayers = new List<PlayerToCorrect>();

        internal CorrectPlayerUI(Raid r, List<CheckPlayer> players)
        {
            InitializeComponent();
            ApplyLanguage(Language.Data);
            PlayerToCorrect.SetRaid(r);
            foreach(var p in players)
            {
                var element = new PlayerToCorrect(p);
                ShowenPlayers.Add(element);
                flpMain.Controls.Add(element);
            }
        }

        private void CorrectPlayerUI_Load(object sender, EventArgs e)
        {

        }

        private void ApplyLanguage(ILanguage data)
        {
            Text = data.CorrectPlayer;
            lblTitle.Text = data.CorrectPlayerTitle;
            bntDone.Text = data.CorrectPlayerDone;
        }

        private void UpdatePlayers()
        {
            foreach (var element in ShowenPlayers)
            {
                element.UpdatePlayer();
            }
        }

        private void BntDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CorrectPlayerUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdatePlayers();
        }
    }
}

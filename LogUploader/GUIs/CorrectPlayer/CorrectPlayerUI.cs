using LogUploader.Data.RaidOrgaPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LogUploader.Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker;

namespace LogUploader.GUIs.CorrectPlayer
{
    public partial class CorrectPlayerUI : Form
    {
        private List<PlayerToCorrect> ShowenPlayers = new List<PlayerToCorrect>();

        public CorrectPlayerUI(Raid r, List<CheckPlayer> players)
        {
            InitializeComponent();
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

        private void UpdatePlayers()
        {
            foreach (var element in ShowenPlayers)
            {
                element.UpdatePlayer();
            }
        }

        private void bntDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //TODO implement and make visible again
        }

        private void CorrectPlayerUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdatePlayers();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Languages;
using LogUploader.Data.RaidOrgaPlus;

namespace LogUploader.GUIs.CorrectPlayer
{
    public partial class PlayerToCorrect : UserControl
    {
        private static Raid Raid;
        private static List<Account> AllHelper = new List<Account>();
        private Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.CheckPlayer Player;

        public static void SetRaid(Raid r)
        {
            Raid = r;
            AllHelper.Clear();
            AllHelper.AddRange(r.Helper);
            AllHelper.AddRange(r.Inviteable);
        }

        public PlayerToCorrect(Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.CheckPlayer player)
        {
            Player = player;
            InitializeComponent();
            ApplyLanguage(Language.Data);
            BindComboBoxes();

            lblAccountName.Text = player.Player.AccountName;
            switch (player.Player.Type)
            {
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.MEMBER:
                    rbMember.Checked = true;
                    cbMember.Enabled = true;
                    cbMember.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.HELPER:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbMember.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.INVITEABLE:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbMember.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.LFG:
                    rbLFG.Enabled = true;
                    break;
            }

            TimerCBMember.Interval = 2000;
            TimerCBMember.Tick += (sender, e) => updateDataCbMember();
            TimerCBHelper.Interval = 2000;
            TimerCBHelper.Tick += (sender, e) => updateDataCbHelper();

            //TODO localize
            lblStatus.Text = "OK";

        }

        internal void UpdatePlayer()
        {
            if (rbMember.Checked)
            {
                Player.BecomesAccount = (Account)cbMember.SelectedItem;
                Player.BecomesType = Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.MEMBER;
            }
            else if (rbHelper.Checked)
            {
                Player.BecomesAccount = (Account)cbHelper.SelectedItem;
                if (Raid.IsHelper(Player.BecomesAccount.AccountName))
                    Player.BecomesType = Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.HELPER;
                else
                    Player.BecomesType = Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.INVITEABLE;
            }
            else
                Player.BecomesType = Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.LFG;
        }

        private void BindComboBoxes()
        {
            cbMember.DisplayMember = "UIString";
            cbMember.DataSource = Raid.Players;

            cbHelper.DisplayMember = "UIString";
            cbMember.DataSource = AllHelper;
        }

        private void ApplyLanguage(ILanguage data)
        {
            //TODO ApplyLanguage
        }

        private void rbMember_CheckedChanged(object sender, EventArgs e)
        {
            //TODO localize
            lblStatus.Text = "???";

            cbMember.Enabled = false;
            cbHelper.Enabled = false;

            if (sender == rbMember)
                cbMember.Enabled = true;
            else if (sender == rbHelper)
                cbHelper.Enabled = true;

        }

        private Timer TimerCBMember = new Timer();

        private void cbMember_TextUpdate(object sender, EventArgs e)
        {
            if (cbMember.SelectedText.Length < 3)
            {
                cbMember.DataSource = new List<Account>();
                return;
            }
            TimerCBMember.Stop();
            TimerCBMember.Start();

        }

        private void updateDataCbMember()
        {
            TimerCBMember.Stop();
            var items = Raid.Players.Where(p => p.UIString.ToLowerInvariant().Contains(cbMember.SelectedText.ToLowerInvariant())).ToList();
            cbMember.DataSource = items;
        }

        private Timer TimerCBHelper = new Timer();

        private void cbHelper_TextUpdate(object sender, EventArgs e)
        {
            if (cbHelper.SelectedText.Length < 3)
            {
                cbHelper.DataSource = new List<Account>();
                return;
            }
            TimerCBHelper.Stop();
            TimerCBHelper.Start();
        }
        private void updateDataCbHelper()
        {
            TimerCBHelper.Stop();
            var items = AllHelper.Where(p => p.UIString.ToLowerInvariant().Contains(cbHelper.SelectedText.ToLowerInvariant())).ToList();
            cbHelper.DataSource = items;
        }
    }
}

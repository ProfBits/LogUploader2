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
        private readonly Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.CheckPlayer Player;

        public static void SetRaid(Raid r)
        {
            Raid = r;
            AllHelper.Clear();
            AllHelper.AddRange(r.Helper);
            AllHelper.AddRange(r.Inviteable);
            AllHelper = AllHelper.Distinct(new AccountEqualityComparer()).ToList();
        }

        private class AccountEqualityComparer : IEqualityComparer<Account>
        {
            public bool Equals(Account x, Account y) =>
                x.ID == y.ID;

            public int GetHashCode(Account obj) => obj.ID.GetHashCode();
        }

        public PlayerToCorrect(Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.CheckPlayer player)
        {
            Player = player;
            InitializeComponent();
            ApplyLanguage(Language.Data);
            BindComboBoxes();

            lblAccountName.Text = player.Player.AccountName;
            switch (player.BecomesType)
            {
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.MEMBER:
                    rbMember.Checked = true;
                    cbMember.Enabled = true;
                    cbMember.Visible = true;
                    cbMember.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.HELPER:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbHelper.Visible = true;
                    cbHelper.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.INVITEABLE:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbHelper.Visible = true;
                    cbHelper.SelectedItem = player.BecomesAccount;
                    break;
                case Helper.RaidOrgaPlus.RaidOrgaPlusDataWorker.PlayerType.LFG:
                    rbLFG.Checked = true;
                    break;
            }
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
            cbHelper.DataSource = AllHelper;
        }

        private void ApplyLanguage(ILanguage data)
        {
            //TODO ApplyLanguage
        }

        private void rbMember_CheckedChanged(object sender, EventArgs e)
        {
            cbMember.Enabled = false;
            cbHelper.Enabled = false;
            cbMember.Visible = false;
            cbHelper.Visible = false;

            if (sender == rbMember)
            {
                cbMember.Enabled = true;
                cbMember.Visible = true;
            }
            else if (sender == rbHelper)
            {
                cbHelper.Enabled = true;
                cbHelper.Visible = true;
            }

        }
    }
}

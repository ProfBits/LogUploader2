using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Localisation;
using LogUploader.Tools.RaidOrgaPlus.Data;

namespace LogUploader.Tools.RaidOrgaPlus.GUI
{
    public partial class PlayerToCorrect : UserControl
    {
        private static Raid Raid;
        private static List<Account> AllHelper = new List<Account>();
        private readonly CheckPlayer Player;

        internal static void SetRaid(Raid r)
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

        internal PlayerToCorrect(CheckPlayer player)
        {
            Player = player;
            InitializeComponent();
            ApplyLanguage(Language.Data);
            BindComboBoxes();

            lblAccountName.Text = player.Player.AccountName;
            switch (player.BecomesType)
            {
                case PlayerType.MEMBER:
                    rbMember.Checked = true;
                    cbMember.Enabled = true;
                    cbMember.Visible = true;
                    cbMember.SelectedItem = player.BecomesAccount;
                    break;
                case PlayerType.HELPER:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbHelper.Visible = true;
                    cbHelper.SelectedItem = player.BecomesAccount;
                    break;
                case PlayerType.INVITEABLE:
                    rbHelper.Checked = true;
                    cbHelper.Enabled = true;
                    cbHelper.Visible = true;
                    cbHelper.SelectedItem = player.BecomesAccount;
                    break;
                case PlayerType.LFG:
                    rbLFG.Checked = true;
                    break;
            }
        }

        internal void UpdatePlayer()
        {
            if (rbMember.Checked)
            {
                Player.BecomesAccount = (Account)cbMember.SelectedItem;
                Player.BecomesType = PlayerType.MEMBER;
            }
            else if (rbHelper.Checked)
            {
                Player.BecomesAccount = (Account)cbHelper.SelectedItem;
                if (Raid.IsHelper(Player.BecomesAccount.AccountName))
                    Player.BecomesType = PlayerType.HELPER;
                else
                    Player.BecomesType = PlayerType.INVITEABLE;
            }
            else
                Player.BecomesType = PlayerType.LFG;
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
            gbAccount.Text =  data.PlayerToCorrectPlayer;
            lblAccountNameDes.Text = data.PlayerToCorrectAccount;
            rbMember.Text = data.PlayerToCorrectMember;
            rbHelper.Text = data.PlayerToCorrectHelper;
            rbLFG.Text =  data.PlayerToCorrectLFG;
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

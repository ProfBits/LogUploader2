using LogUploader.Data.RaidOrgaPlus;

namespace LogUploader.Data.RaidOrgaPlus
{
    /// <summary>
    /// Represents a player in the logs to be validated by the user
    /// </summary>
    public class CheckPlayer
    {
        public RoPlusPlayer Player { get; set; }
        public Account BecomesAccount { get; set; }
        public PlayerType BecomesType { get; set; }

        public CheckPlayer(RoPlusPlayer p, Account a)
        {
            Player = p;
            BecomesAccount = a;
            BecomesType = p.Type;
            if (a == null)
                p.Type = PlayerType.LFG;
        }

        internal void Correct(RoPlusPlayer player)
        {
            if (BecomesType == PlayerType.LFG)
                player.SetLFG();
            else
                player.SetAccount(BecomesAccount, BecomesType);
        }
    }
}

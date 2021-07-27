namespace LogUploader.Tools.RaidOrgaPlus.Data
{
    /// <summary>
    /// Represents a player in the logs to be validated by the user
    /// </summary>
    internal class CheckPlayer
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

        public void Correct(RoPlusPlayer player)
        {
            if (BecomesType == PlayerType.LFG)
                player.setLFG();
            else
                player.SetAccount(BecomesAccount, BecomesType);
        }
    }
}

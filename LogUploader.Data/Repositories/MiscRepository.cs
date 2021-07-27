namespace LogUploader.Data.Repositories
{
    internal class MiscRepository : MiscProvider
    {
        private MiscData Data { get; set; }

        public string KillEmote { get => Data.EmoteKill; internal set => Data = new MiscData(value, WipeEmote); }
        public string WipeEmote { get => Data.EmoteWipe; internal set => Data = new MiscData(KillEmote, value); }
        
        internal MiscRepository()
        {
            Data = new MiscData(":white_check_mark:", ":x:");
        }

        public MiscRepository(MiscData data)
        {
            Data = data;
        }
    }
}

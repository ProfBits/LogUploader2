namespace LogUploader.Data.StaticDataLoader
{
    internal class MiscDataRegistrator : IMiscDataRegistrator
    {
        public string KillEmote { get => MiscData.EmoteRaidKill; }
        public string WipeEmote { get => MiscData.EmoteRaidWipe; }

        public void RegisterKillEmote(string emote)
        {
            MiscData.EmoteRaidKill = emote;
        }

        public void RegisterWipeEmote(string emote)
        {
            MiscData.EmoteRaidWipe = emote;
        }
    }

}

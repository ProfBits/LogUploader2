using System;

using LogUploader.Data.Repositories;

namespace LogUploader.Data.StaticDataLoader
{
    internal class MiscDataRegistrator : IMiscDataRegistrator
    {
        MiscRepository Repo;

        public MiscDataRegistrator(MiscRepository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public string KillEmote { get => Repo.KillEmote; }
        public string WipeEmote { get => Repo.WipeEmote; }

        public void RegisterKillEmote(string emote)
        {
            Repo.KillEmote = emote;
        }

        public void RegisterWipeEmote(string emote)
        {
            Repo.WipeEmote = emote;
        }
    }

}

using System;

namespace LogUploader.Interfaces
{
    public interface IBoss : IEnemy, IAvatar, IEquatable<IBoss>, IEquatable<eBosses>
    {
        string DiscordEmote { get; }
        string EIName { get; }
        string FolderName { get; }
        string FolderNameDE { get; }
        string FolderNameEN { get; }
        int RaidOrgaPlusID { get; }
    }
}
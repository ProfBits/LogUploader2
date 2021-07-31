using LogUploader.Localisation;

namespace LogUploader.Data
{
    public interface BossProvider : EnemyProvider<Boss>
    {
        Boss GetByFolderName(string folderName);
        Boss GetByFolderName(string folderName, eLanguage lang);
        Boss GetByEiName(string eiName);
        Boss GetByRaidOrgaPlusID(int ropID);
        Boss Get(eBosses contained);
    }
}

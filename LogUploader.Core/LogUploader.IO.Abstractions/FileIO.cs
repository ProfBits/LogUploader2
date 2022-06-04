using LogUploader.Injection;

namespace LogUploader.IO;

[Service(ServiceType.Singelton)]
public interface IFileIO
{
    string GetPath(RootFolder rootFolder, string relPath);
    void DeleteFile(RootFolder rootFolder, string relPath);
    bool FileExists(RootFolder rootFolder, string relPath);
}

public enum RootFolder
{
    Custom,
    ProgrammFiles,
    AppDataLocal,
    AppDataRoaming,
    Logs,
    StaticData,
    EliteInsights,
    EliteInsightsOutput,
}
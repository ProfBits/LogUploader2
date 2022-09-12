namespace LogUploader.IO;

internal class FileIO : IFileIO
{
    private readonly IReadOnlyDictionary<RootFolder, string> RootFolderMapping;

    internal FileIO(IReadOnlyDictionary<RootFolder, string> rootFolderMapping)
    {
        RootFolderMapping = rootFolderMapping;
    }

    public void DeleteFile(RootFolder rootFolder, string relPath)
    {
        string fullPath = GetFullPath(rootFolder, relPath);
        File.Delete(fullPath);
    }

    private string GetFullPath(RootFolder rootPath, string subPath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(RootFolderMapping[rootPath], subPath));
        var rel = Path.GetRelativePath(RootFolderMapping[rootPath], fullPath);
        if (rel.StartsWith('.') && !Path.IsPathRooted(rel))
        {
            throw new NotImplementedException("Root folder was left");
        }

        return fullPath;
    }

    public bool FileExists(RootFolder rootFolder, string relPath)
    {
        string fullPath = GetFullPath(rootFolder, relPath);
        return File.Exists(fullPath);
    }

    public string GetPath(RootFolder rootFolder, string relPath)
    {
        return GetFullPath(rootFolder, relPath);
    }
}


using System.IO;
using Contract.FileSystem;

namespace Infrastructure.FileSystem;

public class FileSystemWrapper : IFileSystemWrapper
{
    public bool IsDirectoryExisting(string path)
    {
        return Directory.Exists(path);
    }

    public bool IsFileExisting(string filePath)
    {
        return File.Exists(filePath);
    }

    public void Delete(string filePath)
    {
        File.Delete(filePath);
    }
}
namespace Contract.FileSystem;

public interface IFileSystemWrapper
{
    bool IsDirectoryExisting(string path);
    bool IsFileExisting(string filePath);
    void Delete(string filePath);
}
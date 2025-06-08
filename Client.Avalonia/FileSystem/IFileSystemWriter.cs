namespace Contract.FileSystem;

public interface IFileSystemWriter
{
    Task Write(string text, string path);
}
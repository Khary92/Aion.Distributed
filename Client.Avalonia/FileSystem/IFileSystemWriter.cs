using System.Threading.Tasks;

namespace Client.Avalonia.FileSystem;

public interface IFileSystemWriter
{
    Task Write(string text, string path);
}
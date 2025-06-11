using System.Threading.Tasks;

namespace Client.Desktop.FileSystem;

public interface IFileSystemWriter
{
    Task Write(string text, string path);
}
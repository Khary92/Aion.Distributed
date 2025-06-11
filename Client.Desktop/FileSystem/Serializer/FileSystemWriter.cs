using System.IO;
using System.Threading.Tasks;

namespace Client.Desktop.FileSystem.Serializer;

public class FileSystemWriter : IFileSystemWriter
{
    public Task Write(string text, string path)
    {
        using var writer = new StreamWriter(path);
        writer.Write(text);
        return Task.CompletedTask;
    }
}
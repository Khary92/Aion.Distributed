using System.Threading.Tasks;

namespace Client.Avalonia.FileSystem;

public interface IFileSystemReader
{
    public Task<T> GetObject<T>(string filePath);
}
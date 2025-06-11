using System.Threading.Tasks;

namespace Client.Desktop.FileSystem;

public interface IFileSystemReader
{
    public Task<T> GetObject<T>(string filePath);
}
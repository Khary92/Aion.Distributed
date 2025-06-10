using System.Threading.Tasks;

namespace Contract.FileSystem;

public interface IFileSystemReader
{
    public Task<T> GetObject<T>(string filePath);
}
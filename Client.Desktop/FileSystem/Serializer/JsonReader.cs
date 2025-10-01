using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Desktop.FileSystem.Serializer;

public class JsonReader : IFileSystemReader
{
    public async Task<T> GetObject<T>(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            var jsonText = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(jsonText) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception("Object could not be created!", e);
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;

namespace Client.Desktop.FileSystem.Serializer;

public class JsonReader(IDeserializer deserializer) : IFileSystemReader
{
    public async Task<T> GetObject<T>(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            var jsonText = await reader.ReadToEndAsync();
            return deserializer.Deserialize<T>(jsonText);
        }
        catch (Exception e)
        {
            throw new Exception("Object could not be created!", e);
        }
    }
}
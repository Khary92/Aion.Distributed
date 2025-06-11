using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Avalonia.FileSystem.Serializer;

public class JsonReader : IFileSystemReader
{
    public Task<T> GetObject<T>(string filePath)
    {
        try
        {
            string jsonText;
            using (var reader = new StreamReader(filePath))
            {
                jsonText = reader.ReadToEnd();
            }

            return Task.FromResult(JsonSerializer.Deserialize<T>(jsonText) ?? throw new InvalidOperationException());
        }
        catch (Exception e)
        {
            throw new Exception("Object could not be created!", e);
        }
    }
}
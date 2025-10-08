using System.Runtime.Serialization;
using System.Text.Json;

namespace Client.Desktop.FileSystem.Serializer;

public class Deserializer : IDeserializer
{
    public T Deserialize<T>(string jsonText)
    {
        var result = JsonSerializer.Deserialize<T>(jsonText);
        return result ?? throw new SerializationException();
    }
}
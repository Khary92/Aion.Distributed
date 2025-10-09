using System.Runtime.Serialization;
using System.Text.Json;

namespace Client.Desktop.FileSystem.Serializer;

public class SerializationService : ISerializationService
{
    public T Deserialize<T>(string jsonText)
    {
        var result = JsonSerializer.Deserialize<T>(jsonText);
        return result ?? throw new SerializationException();
    }

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
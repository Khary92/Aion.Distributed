namespace Client.Desktop.FileSystem.Serializer;

public interface IDeserializer
{
    T Deserialize<T>(string jsonText);
}
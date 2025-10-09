namespace Client.Desktop.FileSystem.Serializer;

public interface ISerializationService
{
    T Deserialize<T>(string jsonText);
    string Serialize(object obj);
}
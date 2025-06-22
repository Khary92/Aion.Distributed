using Google.Protobuf.Collections;

namespace Proto.Shared;

public static class MapperExtensions
{
    public static List<Guid> ToGuidList(this RepeatedField<string> stringGuids)
    {
        var guids = new List<Guid>();
        foreach (var id in stringGuids)
            if (Guid.TryParse(id, out var guid))
                guids.Add(guid);

        return guids;
    }

    public static RepeatedField<string> ToRepeatedField(this List<Guid> guids)
    {
        var repeatedField = new RepeatedField<string>();
        foreach (var id in guids) repeatedField.Add(id.ToString());

        return repeatedField;
    }
}
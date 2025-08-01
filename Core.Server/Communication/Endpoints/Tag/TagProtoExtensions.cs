using Core.Server.Communication.Records.Commands.Entities.Tags;
using Proto.Command.Tags;
using Proto.DTO.Tag;
using Proto.Notifications.Tag;
using Proto.Requests.Tags;

namespace Core.Server.Communication.Endpoints.Tag;

public static class TagProtoExtensions
{
    public static CreateTagCommand ToCommand(this CreateTagCommandProto proto) => new(Guid.Parse(proto.TagId),
        proto.Name, Guid.Parse(proto.TraceData.TraceId));

    public static TagNotification ToNotification(this CreateTagCommand proto) => new()
    {
        TagCreated = new TagCreatedNotification
        {
            TagId = proto.TagId.ToString(),
            Name = proto.Name,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static UpdateTagCommand ToCommand(this UpdateTagCommandProto proto) => new(Guid.Parse(proto.TagId),
        proto.Name, Guid.Parse(proto.TraceData.TraceId));

    public static TagNotification ToNotification(this UpdateTagCommand proto) => new()
    {
        TagUpdated = new TagUpdatedNotification
        {
            TagId = proto.TagId.ToString(),
            Name = proto.Name,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static TagProto ToProto(this Domain.Entities.Tag tag) => new()
    {
        TagId = tag.TagId.ToString(),
        Name = tag.Name
    };

    public static TagListProto ToProtoList(this List<Domain.Entities.Tag> tags)
    {
        var tagProtos = new TagListProto();

        foreach (var tag in tags) tagProtos.Tags.Add(tag.ToProto());

        return tagProtos;
    }
}
using Proto.Command.Tags;
using Proto.DTO.TraceData;
using Proto.Notifications.Tag;
using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.Communication.Tags.Records;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tags;

public static class TagExtensions
{
    public static CreateTagCommandProto ToProto(this WebCreateTagCommand command) => new()
    {
        TagId = command.TagId.ToString(),
        Name = command.Name,
        TraceData = new TraceDataProto()
        {
            TraceId = command.TraceId.ToString()
        }
    };

    public static UpdateTagCommandProto ToProto(this WebUpdateTagCommand command) => new()
    {
        TagId = command.TagId.ToString(),
        Name = command.Name,
        TraceData = new()
        {
            TraceId = command.TraceId.ToString()
        }
    };

    public static NewTagMessage ToWebModel(this TagCreatedNotification notification) =>
        new(new TagWebModel(Guid.Parse(notification.TagId), notification.Name, false),
            Guid.Parse(notification.TraceData.TraceId));

    public static WebTagUpdatedNotification ToNotification(this TagUpdatedNotification notification) =>
        new(Guid.Parse(notification.TagId), notification.Name, Guid.Parse(notification.TraceData.TraceId));
}
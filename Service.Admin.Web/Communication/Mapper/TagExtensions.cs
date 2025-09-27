using Proto.Command.Tags;
using Proto.DTO.TraceData;
using Proto.Notifications.Tag;
using Service.Admin.Web.Communication.Records.Commands;
using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Mapper;

public static class TagExtensions
{
    public static CreateTagCommandProto ToProto(this WebCreateTagCommand command)
    {
        return new CreateTagCommandProto
        {
            TagId = command.TagId.ToString(),
            Name = command.Name,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static UpdateTagCommandProto ToProto(this WebUpdateTagCommand command)
    {
        return new UpdateTagCommandProto
        {
            TagId = command.TagId.ToString(),
            Name = command.Name,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static NewTagMessage ToWebModel(this TagCreatedNotification notification)
    {
        return new NewTagMessage(new TagWebModel(Guid.Parse(notification.TagId), notification.Name),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebTagUpdatedNotification ToNotification(this TagUpdatedNotification notification)
    {
        return new WebTagUpdatedNotification(Guid.Parse(notification.TagId), notification.Name,
            Guid.Parse(notification.TraceData.TraceId));
    }
}
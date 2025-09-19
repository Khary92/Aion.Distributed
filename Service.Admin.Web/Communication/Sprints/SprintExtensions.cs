using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.DTO.TraceData;
using Proto.Notifications.Sprint;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Communication.Sprints.Records;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints;

public static class SprintExtensions
{
    public static CreateSprintCommandProto ToProto(this WebCreateSprintCommand proto)
    {
        return new CreateSprintCommandProto
        {
            SprintId = proto.SprintId.ToString(),
            Name = proto.Name,
            StartTime = Timestamp.FromDateTimeOffset(proto.StartTime),
            EndTime = Timestamp.FromDateTimeOffset(proto.EndTime),
            IsActive = proto.IsActive,
            TraceData = new TraceDataProto
            {
                TraceId = proto.TraceId.ToString()
            }
        };
    }

    public static UpdateSprintDataCommandProto ToProto(this WebUpdateSprintDataCommand command)
    {
        return new UpdateSprintDataCommandProto
        {
            SprintId = command.SprintId.ToString(),
            Name = command.Name,
            StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
            EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static SetSprintActiveStatusCommandProto ToProto(this WebSetSprintActiveStatusCommand command)
    {
        return new SetSprintActiveStatusCommandProto
        {
            SprintId = command.SprintId.ToString(),
            IsActive = command.IsActive,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static NewSprintMessage ToWebModel(this SprintCreatedNotification notification)
    {
        return new NewSprintMessage(new SprintWebModel(
            Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            notification.TicketIds.ToGuidList()), Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebSprintDataUpdatedNotification ToNotification(this SprintDataUpdatedNotification notification)
    {
        return new WebSprintDataUpdatedNotification(Guid.Parse(notification.SprintId), notification.Name,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebSetSprintActiveStatusNotification ToNotification(
        this SprintActiveStatusSetNotification notification)
    {
        return new WebSetSprintActiveStatusNotification(Guid.Parse(notification.SprintId),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebAddTicketToActiveSprintNotification ToNotification(
        this TicketAddedToActiveSprintNotification notification)
    {
        return new WebAddTicketToActiveSprintNotification(Guid.Parse(notification.TicketId),
            Guid.Parse(notification.TraceData.TraceId));
    }
}
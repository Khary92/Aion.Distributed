using System;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Notifications.Sprint;
using Proto.Requests.Sprints;

namespace Client.Desktop.Communication.Requests.Sprint;

public static class SprintRequestExtensions
{
    public static GetActiveSprintRequestProto ToProto(this ClientGetActiveSprintRequest request) => new();
    public static GetAllSprintsRequestProto ToProto(this ClientGetAllSprintsRequest request) => new();
    
    public static SprintClientModel ToWebModel(this SprintCreatedNotification notification)
    {
        return new SprintClientModel(Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            notification.TicketIds.ToGuidList());
    }
}
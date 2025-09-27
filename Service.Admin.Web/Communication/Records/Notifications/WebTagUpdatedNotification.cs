namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebTagUpdatedNotification(Guid TagId, string Name, Guid TraceId);
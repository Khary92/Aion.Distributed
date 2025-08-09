namespace Service.Admin.Web.Communication.Wrappers;

public record NewTimeSlotDependenciesMessage(Guid TicketId, Guid TimeSlotId, Guid StatisticsDataId, Guid TraceId);
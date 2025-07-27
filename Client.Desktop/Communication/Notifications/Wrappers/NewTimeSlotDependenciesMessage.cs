using System;

namespace Client.Desktop.Communication.Notifications.Wrappers;

public record NewTimeSlotDependenciesMessage(Guid TicketId, Guid TimeSlotId, Guid StatisticsDataId);
using System;

namespace Client.Desktop.Communication.NotificationWrappers;

public record NewTimeSlotDependenciesMessage(Guid TicketId, Guid TimeSlotId, Guid StatisticsDataId);
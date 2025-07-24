using System;

namespace Client.Desktop.Communication.Notifications.NotificationWrappers;

public record NewTimeSlotDependenciesMessage(Guid TicketId, Guid TimeSlotId, Guid StatisticsDataId);
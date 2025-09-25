using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.Client.Records;

public record ClientTrackingControlCreatedNotification(
    StatisticsDataClientModel StatisticsData,
    TicketClientModel Ticket,
    TimeSlotClientModel TimeSlot,
    Guid TraceId);
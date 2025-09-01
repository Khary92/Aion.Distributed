using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.UseCase.Records;

public record ClientTimeSlotControlCreatedNotification(
    StatisticsDataClientModel StatisticsData,
    TicketClientModel Ticket,
    TimeSlotClientModel TimeSlot);
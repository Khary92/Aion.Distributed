using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.UseCase.Records;

public record ClientGetTrackingControlResponse(
    StatisticsDataClientModel StatisticsData,
    TicketClientModel Ticket,
    TimeSlotClientModel TimeSlot);
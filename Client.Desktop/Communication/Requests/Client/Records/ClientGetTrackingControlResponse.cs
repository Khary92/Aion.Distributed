using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.Client.Records;

public record ClientGetTrackingControlResponse(
    StatisticsDataClientModel StatisticsData,
    TicketClientModel Ticket,
    TimeSlotClientModel TimeSlot);
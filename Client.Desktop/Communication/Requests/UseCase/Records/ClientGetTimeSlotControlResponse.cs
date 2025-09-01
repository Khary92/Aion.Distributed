using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.UseCase.Records;

public record ClientGetTimeSlotControlResponse(
    StatisticsDataClientModel StatisticsData,
    TicketClientModel Ticket,
    TimeSlotClientModel TimeSlot);
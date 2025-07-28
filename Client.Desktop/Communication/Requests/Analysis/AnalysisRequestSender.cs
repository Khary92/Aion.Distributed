using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Proto;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;
using Proto.Requests.AnalysisData;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Analysis;

public class AnalysisRequestSender(ITagRequestSender requestSender) : IAnalysisRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly AnalysisRequestService.AnalysisRequestServiceClient _client = new(Channel);

    public async Task<AnalysisBySprintDecorator> Send(GetSprintAnalysisById request)
    {
        var response = await _client.GetSprintAnalysisAsync(request);

        var analysisBySprint = new AnalysisBySprint
        {
            SprintName = response.SprintName,
            TimeSlots = GetTimeSlots(response.TimeSlots),
            StatisticsData = GetStatisticsData(response.StatisticsData),
            Tickets = GetTickets(response.Tickets),
            ProductiveTags = response.ProductiveTags.ToDictionary(),
            NeutralTags = response.NeutralTags.ToDictionary(),
            UnproductiveTags = response.UnproductiveTags.ToDictionary()
        };

        return new AnalysisBySprintDecorator(analysisBySprint);
    }

    public async Task<AnalysisByTicketDecorator> Send(GetTicketAnalysisById request)
    {
        var response = await _client.GetTicketAnalysisAsync(request);

        var analysisByTicket = new AnalysisByTicket
        {
            TicketName = response.TicketName,
            TimeSlots = GetTimeSlots(response.TimeSlots),
            StatisticData = GetStatisticsData(response.StatisticData)
        };

        return new AnalysisByTicketDecorator(analysisByTicket, requestSender);
    }

    public async Task<AnalysisByTagDecorator> Send(GetTagAnalysisById request)
    {
        var response = await _client.GetTagAnalysisAsync(request);

        var analysisByTag = new AnalysisByTag
        {
            TimeSlots = GetTimeSlots(response.TimeSlots),
            StatisticsData = GetStatisticsData(response.StatisticsData)
        };

        return new AnalysisByTagDecorator(analysisByTag);
    }

    private static List<TimeSlotClientModel> GetTimeSlots(RepeatedField<TimeSlotProto> timeSlotProtos)
    {
        return timeSlotProtos.Select(timeSlot => new TimeSlotClientModel(Guid.Parse(timeSlot.TimeSlotId),
            Guid.Parse(timeSlot.WorkDayId), Guid.Parse(timeSlot.SelectedTicketId),
            timeSlot.StartTime.ToDateTimeOffset(), timeSlot.EndTime.ToDateTimeOffset(),
            GetParsedGuids(timeSlot.NoteIds), timeSlot.IsTimerRunning)).ToList();
    }

    private static List<StatisticsDataClientModel> GetStatisticsData(
        RepeatedField<StatisticsDataProto> statisticsDataProtos)
    {
        return statisticsDataProtos.Select(statisticsDataProto => new StatisticsDataClientModel(
            Guid.Parse(statisticsDataProto.StatisticsId), Guid.Parse(statisticsDataProto.TimeSlotId),
            GetParsedGuids(statisticsDataProto.TagIds), statisticsDataProto.IsProductive, statisticsDataProto.IsNeutral,
            statisticsDataProto.IsUnproductive)).ToList();
    }

    private static List<TicketClientModel> GetTickets(RepeatedField<TicketProto> ticketProtos)
    {
        return ticketProtos.Select(ticketProto => new TicketClientModel(Guid.Parse(ticketProto.TicketId),
            ticketProto.Name,
            ticketProto.BookingNumber, ticketProto.Documentation, GetParsedGuids(ticketProto.SprintIds))).ToList();
    }

    private static List<Guid> GetParsedGuids(RepeatedField<string> guidStrings)
    {
        return guidStrings
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();
    }
}
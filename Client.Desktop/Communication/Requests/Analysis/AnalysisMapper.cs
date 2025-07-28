using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Google.Protobuf.Collections;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Analysis;

public class AnalysisMapper(ITagRequestSender requestSender) : IAnalysisMapper
{
    public AnalysisBySprintDecorator Create(AnalysisBySprintProto proto)
    {
        var analysisBySprint = new AnalysisBySprint
        {
            SprintName = proto.SprintName,
            TimeSlots = GetTimeSlots(proto.TimeSlots),
            StatisticsData = GetStatisticsData(proto.StatisticsData),
            Tickets = GetTickets(proto.Tickets),
            ProductiveTags = proto.ProductiveTags.ToDictionary(),
            NeutralTags = proto.NeutralTags.ToDictionary(),
            UnproductiveTags = proto.UnproductiveTags.ToDictionary()
        };

        return new AnalysisBySprintDecorator(analysisBySprint);
    }

    public AnalysisByTicketDecorator Create(AnalysisByTicketProto proto)
    {
        var analysisByTicket = new AnalysisByTicket
        {
            TicketName = proto.TicketName,
            TimeSlots = GetTimeSlots(proto.TimeSlots),
            StatisticData = GetStatisticsData(proto.StatisticData)
        };

        return new AnalysisByTicketDecorator(analysisByTicket, requestSender);
    }

    public AnalysisByTagDecorator Create(AnalysisByTagProto proto)
    {
        var analysisByTag = new AnalysisByTag
        {
            TimeSlots = GetTimeSlots(proto.TimeSlots),
            StatisticsData = GetStatisticsData(proto.StatisticsData)
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
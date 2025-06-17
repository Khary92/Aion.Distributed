using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;
using Proto.Requests.AnalysisData;

namespace Service.Server.Mock.Analysis;

public class MockAnalysisRequestService : AnalysisRequestService.AnalysisRequestServiceBase
{
    public override Task<AnalysisBySprintProto> GetSprintAnalysis(GetSprintAnalysisById request,
        ServerCallContext context)
    {
        var response = new AnalysisBySprintProto
        {
            SprintName = "Sprint 42",
            TimeSlots =
            {
                new TimeSlotProto
                {
                    TimeSlotId = MockIds.TimeSlotId1,
                    WorkDayId = Guid.NewGuid().ToString(),
                    SelectedTicketId = MockIds.TicketId1,
                    StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-2)),
                    EndTime = Timestamp.FromDateTime(DateTime.UtcNow),
                    NoteIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    IsTimerRunning = false
                }
            },
            StatisticsData =
            {
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId1,
                    TagIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    IsProductive = true,
                    IsNeutral = false,
                    IsUnproductive = false
                },
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId2,
                    TagIds = { Guid.NewGuid().ToString() },
                    IsProductive = false,
                    IsNeutral = true,
                    IsUnproductive = false
                }
            },
            Tickets =
            {
                new TicketProto
                {
                    TicketId = MockIds.TicketId1,
                    Name = "Implement Sprint Analysis",
                    BookingNumber = "BN-998877",
                    Documentation = "Implemented sprint-level breakdown of time slots.",
                    SprintIds = { Guid.NewGuid().ToString() }
                }
            },
            ProductiveTags =
            {
                { "focus", 4 },
                { "deep_work", 3 }
            },
            NeutralTags =
            {
                { "context_switch", 2 }
            },
            UnproductiveTags =
            {
                { "distraction", 1 },
                { "meeting", 2 }
            }
        };

        return Task.FromResult(response);
    }

    public override Task<AnalysisByTagProto> GetTagAnalysis(GetTagAnalysisById request, ServerCallContext context)
    {
        var response = new AnalysisByTagProto
        {
            TimeSlots =
            {
                new TimeSlotProto
                {
                    TimeSlotId = MockIds.TimeSlotId1,
                    WorkDayId = Guid.NewGuid().ToString(),
                    SelectedTicketId = MockIds.TicketId1,
                    StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-3)),
                    EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-2)),
                    NoteIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    IsTimerRunning = false
                }
            },
            StatisticsData =
            {
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId1,
                    TagIds = { Guid.NewGuid().ToString() },
                    IsProductive = false,
                    IsNeutral = false,
                    IsUnproductive = true
                },
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId2,
                    TagIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    IsProductive = true,
                    IsNeutral = false,
                    IsUnproductive = false
                }
            }
        };

        return Task.FromResult(response);
    }

    public override Task<AnalysisByTicketProto> GetTicketAnalysis(GetTicketAnalysisById request,
        ServerCallContext context)
    {
        var response = new AnalysisByTicketProto
        {
            TicketName = "TICKET-123: Improve Data Insights",
            TimeSlots =
            {
                new TimeSlotProto
                {
                    TimeSlotId = MockIds.TimeSlotId1,
                    WorkDayId = Guid.NewGuid().ToString(),
                    SelectedTicketId = MockIds.TicketId1,
                    StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-4)),
                    EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-3)),
                    NoteIds = { Guid.NewGuid().ToString() },
                    IsTimerRunning = false
                },
                new TimeSlotProto
                {
                    TimeSlotId = MockIds.TimeSlotId2,
                    WorkDayId = Guid.NewGuid().ToString(),
                    SelectedTicketId = MockIds.TicketId1,
                    StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-2)),
                    EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-1)),
                    NoteIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                    IsTimerRunning = false
                }
            },
            StatisticData =
            {
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId1,
                    TagIds = { MockIds.TagId1 },
                    IsProductive = true,
                    IsNeutral = false,
                    IsUnproductive = false
                },
                new StatisticsDataProto
                {
                    StatisticsId = Guid.NewGuid().ToString(),
                    TimeSlotId = MockIds.TimeSlotId2,
                    TagIds = { MockIds.TagId2 },
                    IsProductive = false,
                    IsNeutral = true,
                    IsUnproductive = false
                }
            }
        };

        return Task.FromResult(response);
    }
}
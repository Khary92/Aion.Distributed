using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public static class AnalysisByTicketModelProvider
{
    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    private static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    private static Mock<IRequestSender> CreateRequestSenderMock() => new();

    private static TicketClientModel CreateTicketClientModel() => new(Guid.NewGuid(), "InitialTicketName", "InitialBookingNumber", "Documentation",
        []);


    public static async Task<AnalysisModelFixture<AnalysisByTicketModel>> Create(
        List<TicketClientModel?> initialTickets)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))!
            .ReturnsAsync(initialTickets.ToList()!);

        return await CreateFixture(messenger, requestSender, tracer);
    }
    
    
    public static async Task<AnalysisModelFixture<AnalysisByTicketModel>> Create()
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        var sprintClientModel = CreateTicketClientModel();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTicketAnalysisById>()))
            .ReturnsAsync(new AnalysisByTicketDecorator(CreateAnalysisByTicket()));

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))
            .ReturnsAsync([sprintClientModel]);

        return await CreateFixture(messenger, requestSender, tracer);
    }

    private static async Task<AnalysisModelFixture<AnalysisByTicketModel>> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ITraceCollector> tracer)
    {
        var instance = new AnalysisByTicketModel(messenger, requestSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();
        
        await instance.SetAnalysisByTicket(CreateTicketClientModel());

        return new AnalysisModelFixture<AnalysisByTicketModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger
        };
    }

    private static AnalysisByTicket CreateAnalysisByTicket()
    {
        var timeSlotId = Guid.NewGuid();
        var statisticsId = Guid.NewGuid();

        var statisticsDataClientModel =
            new StatisticsDataClientModel(statisticsId, timeSlotId, new List<Guid>(), true, false, false);

        var timeSlotClientModel = new TimeSlotClientModel(timeSlotId, Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new List<Guid>(), false);


        return new AnalysisByTicket()
        {
            TicketName = "TicketName",
            ProductiveTags = new Dictionary<string, int>() { { "Productive Tag", 2 } },
            NeutralTags = new Dictionary<string, int>() { { "Neutral Tag", 1 } },
            UnproductiveTags = new Dictionary<string, int>() { { "Unproductive Tag", 1 } },
            StatisticData = [statisticsDataClientModel],
            TimeSlots = [timeSlotClientModel]
        };
    }
}
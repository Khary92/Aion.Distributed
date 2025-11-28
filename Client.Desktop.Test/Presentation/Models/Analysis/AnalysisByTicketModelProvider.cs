using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Desktop.Services.Authentication;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public static class AnalysisByTicketModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object,
            CreateTokenServiceMock().Object);
    }

    private static Mock<IGrpcUrlService> CreateGrpcUrlBuilderMock()
    {
        return new Mock<IGrpcUrlService>();
    }

    private static Mock<ITokenService> CreateTokenServiceMock()
    {
        return new Mock<ITokenService>();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    private static TicketClientModel CreateTicketClientModel()
    {
        return new TicketClientModel(Guid.NewGuid(), "InitialTicketName", "InitialBookingNumber", "ChangeDocumentation",
            new List<Guid>());
    }

    public static async Task<AnalysisModelFixture<AnalysisByTicketModel>> Create(
        List<TicketClientModel?> initialTickets)
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))!
            .ReturnsAsync(initialTickets.ToList()!);

        return await CreateFixture(requestSender, tracer, publisherFacade);
    }

    public static async Task<AnalysisModelFixture<AnalysisByTicketModel>> Create()
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();

        var ticketClientModel = CreateTicketClientModel();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTicketAnalysisById>()))
            .ReturnsAsync(new AnalysisByTicketDecorator(CreateAnalysisByTicket()));

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))
            .ReturnsAsync(new List<TicketClientModel?> { ticketClientModel });

        return await CreateFixture(requestSender, tracer, publisherFacade);
    }

    private static async Task<AnalysisModelFixture<AnalysisByTicketModel>> CreateFixture(
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer,
        TestNotificationPublisherFacade publisherFacade)
    {
        var instance = new AnalysisByTicketModel(requestSender.Object, tracer.Object, publisherFacade);

        instance.RegisterMessenger();
        await instance.InitializeAsync();
        await instance.SetAnalysisByTicket(CreateTicketClientModel());

        return new AnalysisModelFixture<AnalysisByTicketModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            NotificationPublisher = publisherFacade
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

        return new AnalysisByTicket
        {
            TicketName = "TicketName",
            ProductiveTags = new Dictionary<string, int> { { "Productive Tag", 2 } },
            NeutralTags = new Dictionary<string, int> { { "Neutral Tag", 1 } },
            UnproductiveTags = new Dictionary<string, int> { { "Unproductive Tag", 1 } },
            StatisticData = new List<StatisticsDataClientModel> { statisticsDataClientModel },
            TimeSlots = new List<TimeSlotClientModel> { timeSlotClientModel }
        };
    }
}
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models;

public sealed class ModelFixture<TModel>
{
    public required TModel Instance { get; init; }
    public required Mock<IRequestSender> RequestSender { get; init; }
    public required Mock<ITraceCollector> Tracer { get; init; }
    public required IMessenger Messenger { get; init; }
}

public static class ModelTestProvider
{
    public static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    public static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    public static Mock<IRequestSender> CreateRequestSenderMock() => new();

    private static SprintClientModel CreateSprintClientModel() => new(Guid.NewGuid(), "InitialSprintName", true,
        DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new List<Guid>());


    public static async Task<ModelFixture<AnalysisBySprintModel>> CreateAnalysisBySprintModelAsync(
        IReadOnlyList<SprintClientModel?> initialSprints)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllSprintsRequest>()))!
            .ReturnsAsync(initialSprints as List<SprintClientModel?>);

        return await CreateFixture(messenger, requestSender, tracer);
    }
    
    public static async Task<ModelFixture<AnalysisBySprintModel>> CreateAnalysisBySprintModelAsync()
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        var sprintClientModel = CreateSprintClientModel();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetSprintAnalysisById>()))
            .ReturnsAsync(new AnalysisBySprintDecorator(CreateAnalysisBySprint(sprintClientModel)));

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllSprintsRequest>()))
            .ReturnsAsync(new List<SprintClientModel?>() { sprintClientModel });

        return await CreateFixture(messenger, requestSender, tracer);
    }
    
    private static async Task<ModelFixture<AnalysisBySprintModel>> CreateFixture(IMessenger messenger, Mock<IRequestSender> requestSender, Mock<ITraceCollector> tracer)
    {
        var instance = new AnalysisBySprintModel(messenger, requestSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new ModelFixture<AnalysisBySprintModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger
        };
    }

    private static AnalysisBySprint CreateAnalysisBySprint(SprintClientModel sprintClientModel)
    {
        var timeSlotId = Guid.NewGuid();
        var statisticsId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        
        var statisticsDataClientModel =
            new StatisticsDataClientModel(statisticsId, timeSlotId, new List<Guid>(), true, false, false);
        
        var timeSlotClientModel = new TimeSlotClientModel(timeSlotId, Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new List<Guid>(), false);

        var ticketClientModel = new TicketClientModel(ticketId, "TicketName", "TicketBookingNumber",
            "TicketDocumentation", new List<Guid>() {sprintClientModel.SprintId});
        
        return new AnalysisBySprint()
        {
            SprintName = sprintClientModel.Name,
            ProductiveTags = new Dictionary<string, int>() {{"Productive Tag", 2}},
            NeutralTags = new Dictionary<string, int>() {{"Neutral Tag", 1}},
            UnproductiveTags = new Dictionary<string, int>() {{"Unproductive Tag", 1}},
            StatisticsData = new List<StatisticsDataClientModel>() {statisticsDataClientModel},
            Tickets = new List<TicketClientModel>() {ticketClientModel},
            TimeSlots = new List<TimeSlotClientModel>() {timeSlotClientModel}
        };
    }
}
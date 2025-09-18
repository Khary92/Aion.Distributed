using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public static class AnalysisByTagModelProvider
{
    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    private static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    private static Mock<IRequestSender> CreateRequestSenderMock() => new();

    private static TagClientModel CreateTagClientModel() => new(Guid.NewGuid(), "InitialTagName", true);


    public static async Task<AnalysisModelFixture<AnalysisByTagModel>> Create(IReadOnlyList<TagClientModel> initialTags)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTagsRequest>()))!
            .ReturnsAsync(initialTags.ToList());
        
        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTagAnalysisById>()))!
            .ReturnsAsync(new AnalysisByTagDecorator(CreateAnalysisByTag()));

        return await CreateFixture(messenger, requestSender, tracer);
    }
    
    private static async Task<AnalysisModelFixture<AnalysisByTagModel>> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ITraceCollector> tracer)
    {
        var instance = new AnalysisByTagModel(messenger, requestSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();
        await instance.SetAnalysisForTag(CreateTagClientModel());
        
        return new AnalysisModelFixture<AnalysisByTagModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger
        };
    }

    private static AnalysisByTag CreateAnalysisByTag()
    {
        var timeSlotId = Guid.NewGuid();
        var statisticsId = Guid.NewGuid();

        var statisticsDataClientModel =
            new StatisticsDataClientModel(statisticsId, timeSlotId, new List<Guid>(), true, false, false);

        var timeSlotClientModel = new TimeSlotClientModel(timeSlotId, Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new List<Guid>(), false);
        
        return new AnalysisByTag()
        {
            StatisticsData = [statisticsDataClientModel],
            TimeSlots = [timeSlotClientModel]
        };
    }
}
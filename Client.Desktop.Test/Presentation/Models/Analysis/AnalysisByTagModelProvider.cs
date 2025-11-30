using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Desktop.Services.Authentication;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public static class AnalysisByTagModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object,
            CreateTokenServiceMock().Object);
    }

    private static Mock<ITokenService> CreateTokenServiceMock()
    {
        return new Mock<ITokenService>();
    }

    private static Mock<IGrpcUrlService> CreateGrpcUrlBuilderMock()
    {
        return new Mock<IGrpcUrlService>();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    private static TagClientModel CreateTagClientModel()
    {
        return new TagClientModel(Guid.NewGuid(), "InitialTagName", true);
    }

    public static async Task<AnalysisModelFixture<AnalysisByTagModel>> Create(IReadOnlyList<TagClientModel> initialTags)
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTagsRequest>()))!
            .ReturnsAsync(initialTags.ToList());

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTagAnalysisById>()))!
            .ReturnsAsync(new AnalysisByTagDecorator(CreateAnalysisByTag()));

        return await CreateFixture(requestSender, tracer, publisherFacade);
    }

    private static async Task<AnalysisModelFixture<AnalysisByTagModel>> CreateFixture(
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer,
        TestNotificationPublisherFacade publisherFacade)
    {
        var instance = new AnalysisByTagModel(requestSender.Object, tracer.Object, publisherFacade);

        instance.RegisterMessenger();
        await instance.InitializeAsync();
        await instance.SetAnalysisForTag(CreateTagClientModel());

        return new AnalysisModelFixture<AnalysisByTagModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            NotificationPublisher = publisherFacade
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

        return new AnalysisByTag
        {
            StatisticsData = [statisticsDataClientModel],
            TimeSlots = [timeSlotClientModel]
        };
    }
}
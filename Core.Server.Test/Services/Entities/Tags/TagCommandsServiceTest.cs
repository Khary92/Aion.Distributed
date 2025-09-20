using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Services.Entities.Tags;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;
using Moq;
using TagNotificationService = Core.Server.Communication.Endpoints.Tag.TagNotificationService;

namespace Core.Server.Test.Services.Entities.Tags;

[TestFixture]
[TestOf(typeof(TagCommandsService))]
public class TagCommandsServiceTest
{
    private Mock<TagNotificationService> _mockTagNotificationService;
    private Mock<IEventStore<TagEvent>> _mockEventStore;
    private Mock<ITagCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private TagCommandsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockTagNotificationService = new Mock<TagNotificationService>();
        _mockEventStore = new Mock<IEventStore<TagEvent>>();
        _mockEventTranslator = new Mock<ITagCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new TagCommandsService(
            _mockEventStore.Object,
            _mockTagNotificationService.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateTagCommand(Guid.NewGuid(), "Tag A", Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TagEvent>()), Times.Once);
    }

    [Test]
    public async Task Update()
    {
        var cmd = new UpdateTagCommand(Guid.NewGuid(), "New Name", Guid.NewGuid());

        await _instance.Update(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TagEvent>()), Times.Once);
    }
}
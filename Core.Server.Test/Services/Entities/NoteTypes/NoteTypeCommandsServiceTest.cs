using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Core.Server.Services.Entities.NoteTypes;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.NoteTypes;
using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Moq;
using NoteTypeNotificationService = Core.Server.Communication.Endpoints.NoteType.NoteTypeNotificationService;

namespace Core.Server.Test.Services.Entities.NoteTypes;

[TestFixture]
[TestOf(typeof(NoteTypeCommandsService))]
public class NoteTypeCommandsServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockNoteTypeNotificationService = new Mock<NoteTypeNotificationService>();
        _mockEventStore = new Mock<IEventStore<NoteTypeEvent>>();
        _mockEventTranslator = new Mock<INoteTypeCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new NoteTypeCommandsService(
            _mockNoteTypeNotificationService.Object,
            _mockEventStore.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    private Mock<NoteTypeNotificationService> _mockNoteTypeNotificationService;
    private Mock<IEventStore<NoteTypeEvent>> _mockEventStore;
    private Mock<INoteTypeCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private NoteTypeCommandsService _instance;

    [Test]
    public async Task Create()
    {
        var createNoteTypeCommand = new CreateNoteTypeCommand(Guid.NewGuid(), "Type A", "#abcdef", Guid.NewGuid());

        await _instance.Create(createNoteTypeCommand);

        _mockEventTranslator.Verify(et => et.ToEvent(createNoteTypeCommand), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<NoteTypeEvent>()), Times.Once);
    }

    [Test]
    public async Task ChangeNoteTypeColorCommand()
    {
        var changeColorCommand = new ChangeNoteTypeColorCommand(Guid.NewGuid(), "#123456", Guid.NewGuid());

        await _instance.ChangeColor(changeColorCommand);

        _mockEventTranslator.Verify(et => et.ToEvent(changeColorCommand), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<NoteTypeEvent>()), Times.Once);
    }

    [Test]
    public async Task ChangeName()
    {
        var changeNameCommand = new ChangeNoteTypeNameCommand(Guid.NewGuid(), "NewName", Guid.NewGuid());

        await _instance.ChangeName(changeNameCommand);

        _mockEventTranslator.Verify(et => et.ToEvent(changeNameCommand), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<NoteTypeEvent>()), Times.Once);
    }
}
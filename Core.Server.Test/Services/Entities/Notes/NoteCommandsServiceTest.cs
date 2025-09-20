
using Core.Server.Communication.Records.Commands.Entities.Note;
using Core.Server.Services.Entities.Notes;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Notes;
using Domain.Events.Note;
using Domain.Interfaces;
using Moq;
using NoteNotificationService = Core.Server.Communication.Endpoints.Note.NoteNotificationService;

namespace Core.Server.Test.Services.Entities.Notes;

[TestFixture]
[TestOf(typeof(NoteCommandsService))]
public class NoteCommandsServiceTest
{
    private Mock<NoteNotificationService> _mockNoteNotificationService;
    private Mock<IEventStore<NoteEvent>> _mockEventStore;
    private Mock<INoteCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private NoteCommandsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockNoteNotificationService = new Mock<NoteNotificationService>();
        _mockEventStore = new Mock<IEventStore<NoteEvent>>();
        _mockEventTranslator = new Mock<INoteCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new NoteCommandsService(
            _mockNoteNotificationService.Object,
            _mockEventStore.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task Create()
    {
        var createNoteCommand = new CreateNoteCommand(Guid.NewGuid(), "Sample Text", Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid());
        
        await _instance.Create(createNoteCommand);
        
        _mockEventTranslator.Verify(et => et.ToEvent(createNoteCommand), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<NoteEvent>()), Times.Once);
    }
    
    [Test]
    public async Task Create_StoresEventAndSendsNotification()
    {
        var updateNoteCommand = new UpdateNoteCommand(Guid.NewGuid(), "Sample Text", Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid());
        
        await _instance.Update(updateNoteCommand);
        
        _mockEventTranslator.Verify(et => et.ToEvent(updateNoteCommand), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<NoteEvent>()), Times.Once);
    }
}
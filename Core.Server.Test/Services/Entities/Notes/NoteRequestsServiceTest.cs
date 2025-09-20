using Core.Server.Services.Entities.Notes;
using Domain.Events.Note;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.Notes;

[TestFixture]
[TestOf(typeof(NoteRequestsService))]
public class NoteRequestsServiceTest
{
    private Mock<IEventStore<NoteEvent>> _mockEventStore;
    private NoteRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<NoteEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<NoteEvent>());

        _instance = new NoteRequestsService(_mockEventStore.Object);
    }

    [Test]
    public async Task GetNotesByTimeSlotId_CallsGetAllEventsAsync()
    {
        await _instance.GetNotesByTimeSlotId(Guid.NewGuid());

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetNotesByTicketId_CallsGetAllEventsAsync()
    {
        await _instance.GetNotesByTicketId(Guid.NewGuid());

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}
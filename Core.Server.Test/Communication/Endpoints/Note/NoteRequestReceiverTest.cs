using Core.Server.Communication.Endpoints.Note;
using Core.Server.Services.Entities.Notes;
using Grpc.Core;
using Moq;
using Proto.Requests.Notes;

namespace Core.Server.Test.Communication.Endpoints.Note;

[TestFixture]
[TestOf(typeof(NoteRequestReceiver))]
public class NoteRequestReceiverTest
{
    [SetUp]
    public void SetUp()
    {
        _mockNoteRequestsService = new Mock<INoteRequestsService>();
        _requestReceiver = new NoteRequestReceiver(_mockNoteRequestsService.Object);
    }

    private Mock<INoteRequestsService> _mockNoteRequestsService;
    private NoteRequestReceiver _requestReceiver;

    [Test]
    public async Task GetNotesByTicketId_ValidRequest_ReturnsExpectedNotes()
    {
        var ticketId = Guid.NewGuid();
        var request = new GetNotesByTicketIdRequestProto { TicketId = ticketId.ToString() };
        var notes = new List<Domain.Entities.Note> { new(), new() };
        _mockNoteRequestsService
            .Setup(service => service.GetNotesByTicketId(ticketId))
            .ReturnsAsync(notes);

        var response = await _requestReceiver.GetNotesByTicketId(request, It.IsAny<ServerCallContext>());

        Assert.That(response, Is.Not.Null);
        Assert.That(notes.Count, Is.EqualTo(response.Notes.Count));
        _mockNoteRequestsService.Verify(service => service.GetNotesByTicketId(ticketId), Times.Once);
    }

    [Test]
    public void GetNotesByTicketId_InvalidGuid_ThrowsException()
    {
        var request = new GetNotesByTicketIdRequestProto { TicketId = "InvalidGuid" };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _requestReceiver.GetNotesByTicketId(request, It.IsAny<ServerCallContext>()));
        _mockNoteRequestsService.Verify(service => service.GetNotesByTicketId(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task GetNotesByTicketId_EmptyResult_ReturnsEmptyResponse()
    {
        var ticketId = Guid.NewGuid();
        var request = new GetNotesByTicketIdRequestProto { TicketId = ticketId.ToString() };
        _mockNoteRequestsService
            .Setup(service => service.GetNotesByTicketId(ticketId))
            .ReturnsAsync(new List<Domain.Entities.Note>());

        var response = await _requestReceiver.GetNotesByTicketId(request, It.IsAny<ServerCallContext>());

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Notes, Is.Empty);
        _mockNoteRequestsService.Verify(service => service.GetNotesByTicketId(ticketId), Times.Once);
    }

    [Test]
    public async Task GetNotesByTimeSlotId_ValidRequest_ReturnsExpectedNotes()
    {
        var timeSlotId = Guid.NewGuid();
        var request = new GetNotesByTimeSlotIdRequestProto { TimeSlotId = timeSlotId.ToString() };
        var notes = new List<Domain.Entities.Note> { new(), new() };
        _mockNoteRequestsService
            .Setup(service => service.GetNotesByTimeSlotId(timeSlotId))
            .ReturnsAsync(notes);

        var response = await _requestReceiver.GetNotesByTimeSlotId(request, It.IsAny<ServerCallContext>());

        Assert.That(response, Is.Not.Null);
        Assert.That(notes.Count, Is.EqualTo(response.Notes.Count));
        _mockNoteRequestsService.Verify(service => service.GetNotesByTimeSlotId(timeSlotId), Times.Once);
    }

    [Test]
    public void GetNotesByTimeSlotId_InvalidGuid_ThrowsException()
    {
        var request = new GetNotesByTimeSlotIdRequestProto { TimeSlotId = "InvalidGuid" };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _requestReceiver.GetNotesByTimeSlotId(request, It.IsAny<ServerCallContext>()));
        _mockNoteRequestsService.Verify(service => service.GetNotesByTimeSlotId(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task GetNotesByTimeSlotId_EmptyResult_ReturnsEmptyResponse()
    {
        var timeSlotId = Guid.NewGuid();
        var request = new GetNotesByTimeSlotIdRequestProto { TimeSlotId = timeSlotId.ToString() };
        _mockNoteRequestsService
            .Setup(service => service.GetNotesByTimeSlotId(timeSlotId))
            .ReturnsAsync(new List<Domain.Entities.Note>());

        var response = await _requestReceiver.GetNotesByTimeSlotId(request, It.IsAny<ServerCallContext>());

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Notes, Is.Empty);
        _mockNoteRequestsService.Verify(service => service.GetNotesByTimeSlotId(timeSlotId), Times.Once);
    }
}
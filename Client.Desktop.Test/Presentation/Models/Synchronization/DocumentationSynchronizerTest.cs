using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Synchronization;

[TestFixture]
[TestOf(typeof(DocumentationSynchronizer))]
public class DocumentationSynchronizerTest
{
    private DocumentationSynchronizer _synchronizer;
    private Mock<ICommandSender> _commandSenderMock;
    private Mock<IRequestSender> _requestSenderMock;
    private Guid _ticketId;
    private TicketReplayDecorator _ticketReplayDecorator;

    [SetUp]
    public void SetUp()
    {
        _commandSenderMock = new Mock<ICommandSender>();
        _synchronizer = new DocumentationSynchronizer(_commandSenderMock.Object);
        _requestSenderMock = new Mock<IRequestSender>();
        _ticketId = Guid.NewGuid();
        _ticketReplayDecorator = new TicketReplayDecorator(_requestSenderMock.Object,
            new TicketClientModel(_ticketId, "Test Name", "123", "Initial Documentation", new List<Guid>()));
    }

    [Test]
    public void RegisterWhenCalledAddsTicketReplayDecoratorToTicketDecoratorsById()
    {
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);

        Assert.That(_synchronizer.GetDecoratorsById(_ticketId).ContainsKey(_ticketReplayDecorator));
    }

    [Test]
    public void SetSynchronizationValueWhenCalledUpdatesDocumentationForTicketAndMarksAsDirty()
    {
        var newDocumentation = "Updated Documentation";
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);

        _synchronizer.SetSynchronizationValue(_ticketId, newDocumentation);

        Assert.That(newDocumentation, Is.EqualTo(_ticketReplayDecorator.DisplayedDocumentation));
        Assert.That(_synchronizer.IsTicketDirty(_ticketId));
    }

    [Test]
    public async Task FireCommandWhenCalledSendsCommandsForDirtyTickets()
    {
        string updatedDocumentation = "Updated Documentation";
        Guid traceId = Guid.NewGuid();
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);
        _synchronizer.SetSynchronizationValue(_ticketId, updatedDocumentation);

        await _synchronizer.FireCommand(traceId);

        _commandSenderMock.Verify(
            cs => cs.Send(It.Is<ClientUpdateTicketDocumentationCommand>(cmd =>
                cmd.TicketId == _ticketId && cmd.Documentation == updatedDocumentation && cmd.TraceId == traceId)),
            Times.Once);

        Assert.That(!_synchronizer.IsTicketDirty(_ticketId));
    }

    [Test]
    public void SetSynchronizationValueWhenCalledWithSameValueDoesNotMarkTicketAsDirty()
    {
        var initialDocumentation = _ticketReplayDecorator.DisplayedDocumentation;
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);

        _synchronizer.SetSynchronizationValue(_ticketId, initialDocumentation);

        Assert.That(!_synchronizer.IsTicketDirty(_ticketId));
    }

    [Test]
    public void FireCommandWhenNoDirtyTicketsDoesNotSendCommands()
    {
        var traceId = Guid.NewGuid();

        Assert.DoesNotThrowAsync(async () => await _synchronizer.FireCommand(traceId));
        _commandSenderMock.Verify(cs => cs.Send(It.IsAny<ClientUpdateTicketDocumentationCommand>()), Times.Never);
    }

    [Test]
    public void FireCommandWhenCalledUpdatesDocumentationInAllAssociatedDecorators()
    {
        var updatedDocumentation = "Updated Documentation";
        var secondDecorator = new TicketReplayDecorator(_requestSenderMock.Object,
            new TicketClientModel(Guid.NewGuid(), "Another Test Name", "456", "", []));
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);
        _synchronizer.Register(_ticketId, secondDecorator);
        _synchronizer.SetSynchronizationValue(_ticketId, updatedDocumentation);

        Assert.That(updatedDocumentation, Is.EqualTo(_ticketReplayDecorator.DisplayedDocumentation));
        Assert.That(updatedDocumentation, Is.EqualTo(secondDecorator.DisplayedDocumentation));
    }

    [Test]
    public void RegisterWhenTicketIdAlreadyExistsAddsDecoratorWithoutOverwritingOthers()
    {
        var anotherDecorator = new TicketReplayDecorator(_requestSenderMock.Object,
            new TicketClientModel(Guid.NewGuid(), "Another Ticket", "", "", []));

        _synchronizer.Register(_ticketId, _ticketReplayDecorator);
        _synchronizer.Register(_ticketId, anotherDecorator);

        var decorators = _synchronizer.GetDecoratorsById(_ticketId);
        Assert.That(decorators.ContainsKey(_ticketReplayDecorator));
        Assert.That(decorators.ContainsKey(anotherDecorator));
    }

    [Test]
    public void FireCommandWhenTicketRemovedDuringOperationGracefullyHandlesIt()
    {
        var traceId = Guid.NewGuid();
        var updatedDocumentation = "Updated Documentation";
        _synchronizer.Register(_ticketId, _ticketReplayDecorator);
        _synchronizer.SetSynchronizationValue(_ticketId, updatedDocumentation);

        _synchronizer.RemoveTicket(_ticketId);

        Assert.DoesNotThrowAsync(async () => await _synchronizer.FireCommand(traceId));
    }
}
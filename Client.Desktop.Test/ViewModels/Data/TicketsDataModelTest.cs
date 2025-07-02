using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Desktop.Models.Data;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.ViewModels.Data;

[TestFixture]
[TestOf(typeof(TicketsDataModel))]
public class TicketsDataModelTest
{
    [SetUp]
    public void SetUp()
    {
        _commandSender = new Mock<ICommandSender>();
        _requestSender = new Mock<IRequestSender>();
        _traceCollector = new Mock<ITraceCollector>();

        _messenger = new StrongReferenceMessenger();
        _ticketDto = new TicketDto(Guid.NewGuid(), InitialName, InitialBookingNumber, InitialDocumentation,
            _initialSprintIds);

        _instance = new TicketsDataModel(_commandSender.Object, _requestSender.Object, _messenger,
            _traceCollector.Object);
        _instance.RegisterMessenger();
    }

    private Mock<ICommandSender> _commandSender;
    private Mock<IRequestSender> _requestSender;
    private Mock<ITraceCollector> _traceCollector;
    private StrongReferenceMessenger _messenger;

    private TicketDto _ticketDto;
    private TicketsDataModel _instance;

    private const string InitialBookingNumber = "initialBookingNumber";
    private const string InitialDocumentation = "initialDocumentation";
    private const string InitialName = "initialName";
    private readonly List<Guid> _initialSprintIds = [Guid.NewGuid()];
}
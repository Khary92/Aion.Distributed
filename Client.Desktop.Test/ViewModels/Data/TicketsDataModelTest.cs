using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Desktop.Models.Data;
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

        _messenger = new StrongReferenceMessenger();
        _ticketDto = new TicketDto(Guid.NewGuid(), InitialName, InitialBookingNumber, InitialDocumentation,
            _initialSprintIds);

        _instance = new TicketsDataModel(_commandSender.Object, _requestSender.Object, _messenger);
        _instance.RegisterMessenger();
    }

    private Mock<ICommandSender> _commandSender;
    private Mock<IRequestSender> _requestSender;
    private StrongReferenceMessenger _messenger;

    private TicketDto _ticketDto;
    private TicketsDataModel _instance;

    private const string InitialBookingNumber = "initialBookingNumber";
    private const string InitialDocumentation = "initialDocumentation";
    private const string InitialName = "initialName";
    private readonly List<Guid> _initialSprintIds = [Guid.NewGuid()];
}
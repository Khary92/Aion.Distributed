using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Documentation;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Documentation;

public static class DocumentationModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object);
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

    private static Mock<INoteViewFactory> CreateNoteViewFactoryMock()
    {
        return new Mock<INoteViewFactory>();
    }

    private static Mock<ITypeCheckBoxViewModelFactory> CreateTypeCheckBoxViewModelFactoryMock()
    {
        return new Mock<ITypeCheckBoxViewModelFactory>();
    }

    private static TicketClientModel CreateTicketModel()
    {
        return new TicketClientModel(Guid.NewGuid(), "name", "bookingNumber", "documentation", []);
    }

    public static async Task<DocumentationModelFixture> Create(List<NoteClientModel> initialNotes,
        List<NoteTypeClientModel> initialNoteTypes,
        List<TicketClientModel> initialTickets)
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();
        var noteViewFactory = CreateNoteViewFactoryMock();
        var typeCheckBoxViewModelFactory = CreateTypeCheckBoxViewModelFactoryMock();
        var ticketClientModel = CreateTicketModel();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))
            .ReturnsAsync(initialTickets);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllNoteTypesRequest>()))
            .ReturnsAsync(initialNoteTypes);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetNotesByTicketIdRequest>()))
            .ReturnsAsync(initialNotes);

        var noteViewModel = new NoteViewModel(null!, requestSender.Object, tracer.Object, publisherFacade)
        {
            Note = initialNotes.First()
        };

        await noteViewModel.InitializeAsync();
        noteViewFactory.Setup(nvf => nvf.Create(It.IsAny<NoteClientModel>()))
            .ReturnsAsync(noteViewModel);

        typeCheckBoxViewModelFactory.Setup(tf => tf.Create(It.IsAny<NoteTypeClientModel>()))
            .Returns(new TypeCheckBoxViewModel((initialNoteTypes.Count != 0 ? initialNoteTypes.First() : null)!));

        return await CreateFixture(requestSender, tracer, publisherFacade, noteViewFactory,
            typeCheckBoxViewModelFactory, ticketClientModel);
    }

    private static async Task<DocumentationModelFixture> CreateFixture(
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer,
        TestNotificationPublisherFacade publisherFacade,
        Mock<INoteViewFactory> noteViewFactory,
        Mock<ITypeCheckBoxViewModelFactory> typeCheckBoxViewModelFactory, TicketClientModel? selectedTicket)
    {
        var instance = new DocumentationModel(requestSender.Object, noteViewFactory.Object,
            typeCheckBoxViewModelFactory.Object, tracer.Object, publisherFacade);

        instance.RegisterMessenger();
        await instance.InitializeAsync();
        instance.SelectedTicket = selectedTicket;
        await instance.UpdateNotesForSelectedTicket();

        return new DocumentationModelFixture
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            NotificationPublisher = publisherFacade,
            NoteViewFactory = noteViewFactory,
            TypeCheckBoxViewModelFactory = typeCheckBoxViewModelFactory
        };
    }

    public sealed class DocumentationModelFixture
    {
        public required DocumentationModel Instance { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
        public required Mock<INoteViewFactory> NoteViewFactory { get; init; }
        public required Mock<ITypeCheckBoxViewModelFactory> TypeCheckBoxViewModelFactory { get; init; }
    }
}
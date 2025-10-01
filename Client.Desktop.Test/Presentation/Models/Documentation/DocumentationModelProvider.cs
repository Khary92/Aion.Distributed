using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Documentation;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Documentation;

public static class DocumentationModelProvider
{
    private static IMessenger CreateMessenger()
    {
        return new WeakReferenceMessenger();
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


    public static async Task<DocumentationModelFixture> Create(List<NoteClientModel> initialNotes,
        List<NoteTypeClientModel> initialNoteTypes,
        List<TicketClientModel> initialTickets)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var noteViewFactory = CreateNoteViewFactoryMock();
        var typeCheckBoxViewModelFactory = CreateTypeCheckBoxViewModelFactoryMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))
            .ReturnsAsync(initialTickets);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllNoteTypesRequest>()))
            .ReturnsAsync(initialNoteTypes);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetNotesByTicketIdRequest>()))
            .ReturnsAsync(initialNotes);

        var noteViewModel = new NoteViewModel(null!, requestSender.Object, tracer.Object)
        {
            Note = initialNotes.First()
        };
        await noteViewModel.Initialize();
        noteViewFactory.Setup(nvf => nvf.Create(It.IsAny<NoteClientModel>()))
            .ReturnsAsync(noteViewModel);

        typeCheckBoxViewModelFactory.Setup(tf => tf.Create(It.IsAny<NoteTypeClientModel>()))
            .Returns(new TypeCheckBoxViewModel((initialNoteTypes.Count != 0 ? initialNoteTypes.First() : null)!));

        return await CreateFixture(messenger, requestSender, tracer, noteViewFactory, typeCheckBoxViewModelFactory);
    }

    private static async Task<DocumentationModelFixture> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ITraceCollector> tracer, Mock<INoteViewFactory> noteViewFactory,
        Mock<ITypeCheckBoxViewModelFactory> typeCheckBoxViewModelFactory)
    {
        var instance = new DocumentationModel(messenger, requestSender.Object, noteViewFactory.Object,
            typeCheckBoxViewModelFactory.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        await instance.UpdateNotesForSelectedTicket();

        return new DocumentationModelFixture
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger,
            NoteViewFactory = noteViewFactory,
            TypeCheckBoxViewModelFactory = typeCheckBoxViewModelFactory
        };
    }

    public sealed class DocumentationModelFixture
    {
        public required DocumentationModel Instance { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<INoteViewFactory> NoteViewFactory { get; init; }
        public required Mock<ITypeCheckBoxViewModelFactory> TypeCheckBoxViewModelFactory { get; init; }
    }
}
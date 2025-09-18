using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Documentation;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public static class DocumentationModelProvider
{
    public sealed class DocumentationModelFixture
    {
        public required DocumentationModel Instance { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<INoteViewFactory> NoteViewFactory { get; init; }
        public required Mock<ITypeCheckBoxViewModelFactory> TypeCheckBoxViewModelFactory { get; init; }
    }

    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    private static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    private static Mock<IRequestSender> CreateRequestSenderMock() => new();
    private static Mock<INoteViewFactory> CreateNoteViewFactoryMock() => new();
    private static Mock<ITypeCheckBoxViewModelFactory> CreateTypeCheckBoxViewModelFactoryMock() => new();

    private static TagClientModel CreateTagClientModel() => new(Guid.NewGuid(), "InitialTagName", true);

    public static async Task<DocumentationModelFixture> Create(List<NoteTypeClientModel> initialNoteTypes,
        List<TicketClientModel> initialTickets)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var noteViewFactory = CreateNoteViewFactoryMock();
        var typeCheckBoxViewModelFactory = CreateTypeCheckBoxViewModelFactoryMock();

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

        return new DocumentationModelFixture()
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger,
            NoteViewFactory = noteViewFactory,
            TypeCheckBoxViewModelFactory = typeCheckBoxViewModelFactory,
        };
    }
}
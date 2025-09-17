using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models;

public sealed class ModelFixture<TModel>
{
    public required TModel Instance { get; init; }
    public required Mock<IRequestSender> RequestSender { get; init; }
    public required Mock<ITraceCollector> Tracer { get; init; }
    public required IMessenger Messenger { get; init; }
}

public static class ModelTestProvider
{
    public static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    public static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    public static Mock<IRequestSender> CreateRequestSenderMock() => new();

    public static async Task<ModelFixture<AnalysisBySprintModel>> CreateAnalysisBySprintModelAsync(
        IReadOnlyList<SprintClientModel?>? initialSprints = null)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllSprintsRequest>()))
            .ReturnsAsync(initialSprints is null
                ? new List<SprintClientModel?>()
                : new List<SprintClientModel?>(initialSprints));

        var instance = new AnalysisBySprintModel(messenger, requestSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new ModelFixture<AnalysisBySprintModel>
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger
        };
    }
}
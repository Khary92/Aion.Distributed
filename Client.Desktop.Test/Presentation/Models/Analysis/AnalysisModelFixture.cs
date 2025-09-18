using Client.Desktop.Communication.Requests;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models;

public sealed class AnalysisModelFixture<TModel>
{
    public required TModel Instance { get; init; }
    public required Mock<IRequestSender> RequestSender { get; init; }
    public required Mock<ITraceCollector> Tracer { get; init; }
    public required IMessenger Messenger { get; init; }
}
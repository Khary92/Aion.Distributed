using Client.Desktop.Communication.Requests;
using Client.Tracing.Tracing.Tracers;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

public sealed class AnalysisModelFixture<TModel>
{
    public required TModel Instance { get; init; }
    public required Mock<IRequestSender> RequestSender { get; init; }
    public required Mock<ITraceCollector> Tracer { get; init; }

    public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
}
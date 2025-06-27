using Client.Desktop.Tracing.Tracing.Tracers.Tag.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
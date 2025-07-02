using Client.Tracing.Tracing.Tracers.Tag.UseCase;

namespace Client.Tracing.Tracing.Tracers.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
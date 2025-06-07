using Contract.Tracing.Tracers.Tag.UseCase;

namespace Contract.Tracing.Tracers.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
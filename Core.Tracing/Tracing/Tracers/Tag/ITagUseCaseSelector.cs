using Core.Server.Tracing.Tracing.Tracers.Tag.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
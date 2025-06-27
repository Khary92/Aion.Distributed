using Client.Desktop.Tracing.Tracing.Tracers.Tag.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Tag;

public class TagUseCaseSelector(
    ICreateTagTraceCollector createTagTraceCollector,
    IUpdateTagTraceCollector updateTagTraceCollector) : ITagUseCaseSelector
{
    public ICreateTagTraceCollector Create => createTagTraceCollector;
    public IUpdateTagTraceCollector Update => updateTagTraceCollector;
}
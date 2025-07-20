using Service.Admin.Tracing.Tracing.Tag.UseCase;

namespace Service.Admin.Tracing.Tracing.Tag;

public class TagUseCaseSelector(
    ICreateTagTraceCollector createTagTraceCollector,
    IUpdateTagTraceCollector updateTagTraceCollector) : ITagUseCaseSelector
{
    public ICreateTagTraceCollector Create => createTagTraceCollector;
    public IUpdateTagTraceCollector Update => updateTagTraceCollector;
}
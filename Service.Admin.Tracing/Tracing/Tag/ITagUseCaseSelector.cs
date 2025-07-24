using Service.Admin.Tracing.Tracing.Tag.UseCase;

namespace Service.Admin.Tracing.Tracing.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
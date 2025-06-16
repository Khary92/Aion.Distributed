using Client.Desktop.Communication.RequiresChange.Tracers.Tag.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Tag;

public interface ITagUseCaseSelector
{
    ICreateTagTraceCollector Create { get; }
    IUpdateTagTraceCollector Update { get; }
}
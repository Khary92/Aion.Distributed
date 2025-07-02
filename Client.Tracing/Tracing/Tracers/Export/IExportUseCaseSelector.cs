using Client.Tracing.Tracing.Tracers.Export.UseCase;

namespace Client.Tracing.Tracing.Tracers.Export;

public interface IExportUseCaseSelector
{
    IExportTraceCollector ToFile { get; }
}
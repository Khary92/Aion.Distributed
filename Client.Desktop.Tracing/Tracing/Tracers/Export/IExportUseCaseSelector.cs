using Client.Desktop.Tracing.Tracing.Tracers.Export.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Export;

public interface IExportUseCaseSelector
{
    IExportTraceCollector ToFile { get; }
}
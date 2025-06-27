using Client.Desktop.Tracing.Tracing.Tracers.Export.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Export;

public class ExportUseCaseSelector(IExportTraceCollector exportTraceCollector) : IExportUseCaseSelector
{
    public IExportTraceCollector ToFile => exportTraceCollector;
}
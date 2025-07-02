using Client.Tracing.Tracing.Tracers.Export.UseCase;

namespace Client.Tracing.Tracing.Tracers.Export;

public class ExportUseCaseSelector(IExportTraceCollector exportTraceCollector) : IExportUseCaseSelector
{
    public IExportTraceCollector ToFile => exportTraceCollector;
}
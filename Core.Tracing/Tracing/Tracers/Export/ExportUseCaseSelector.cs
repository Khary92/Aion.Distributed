using Core.Server.Tracing.Tracing.Tracers.Export.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Export;

public class ExportUseCaseSelector(IExportTraceCollector exportTraceCollector) : IExportUseCaseSelector
{
    public IExportTraceCollector ToFile => exportTraceCollector;
}
using Contract.Tracing.Tracers.Export.UseCase;

namespace Contract.Tracing.Tracers.Export;

public interface IExportUseCaseSelector
{
    IExportTraceCollector ToFile { get; }
}
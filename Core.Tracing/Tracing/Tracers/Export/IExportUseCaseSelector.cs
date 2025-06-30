using Core.Server.Tracing.Tracing.Tracers.Export.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Export;

public interface IExportUseCaseSelector
{
    IExportTraceCollector ToFile { get; }
}
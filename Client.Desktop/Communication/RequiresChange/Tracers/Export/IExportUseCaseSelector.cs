using Client.Desktop.Communication.RequiresChange.Tracers.Export.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Export;

public interface IExportUseCaseSelector
{
    IExportTraceCollector ToFile { get; }
}
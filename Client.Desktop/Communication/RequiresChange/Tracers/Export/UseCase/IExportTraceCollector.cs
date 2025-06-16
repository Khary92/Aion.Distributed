using System;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Export.UseCase;

public interface IExportTraceCollector
{
    void StartUseCase(Type originClassType);
    void PathSettingsInvalid(Type originClassType, object command);
    void ExportSuccessful(Type originClassType);
    void ExceptionOccured(Type originClassType, Exception exception);
}
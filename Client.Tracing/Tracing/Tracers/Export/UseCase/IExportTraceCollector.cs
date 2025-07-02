namespace Client.Tracing.Tracing.Tracers.Export.UseCase;

public interface IExportTraceCollector
{
    Task StartUseCase(Type originClassType);
    Task PathSettingsInvalid(Type originClassType, object command);
    Task ExportSuccessful(Type originClassType);
    Task ExceptionOccured(Type originClassType, Exception exception);
}
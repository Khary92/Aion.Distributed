using Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Note;

public interface INoteUseCaseSelector
{
    ICreateNoteTraceCollector Create { get; }
    IUpdateNoteTraceCollector Update { get; }
}
using Client.Tracing.Tracing.Tracers.Note.UseCase;

namespace Client.Tracing.Tracing.Tracers.Note;

public interface INoteUseCaseSelector
{
    ICreateNoteTraceCollector Create { get; }
    IUpdateNoteTraceCollector Update { get; }
}
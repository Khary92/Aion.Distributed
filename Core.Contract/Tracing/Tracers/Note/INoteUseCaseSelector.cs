using Contract.Tracing.Tracers.Note.UseCase;

namespace Contract.Tracing.Tracers.Note;

public interface INoteUseCaseSelector
{
    ICreateNoteTraceCollector Create { get; }
    IUpdateNoteTraceCollector Update { get; }
}
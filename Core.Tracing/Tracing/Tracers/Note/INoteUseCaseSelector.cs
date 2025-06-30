using Core.Server.Tracing.Tracing.Tracers.Note.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Note;

public interface INoteUseCaseSelector
{
    ICreateNoteTraceCollector Create { get; }
    IUpdateNoteTraceCollector Update { get; }
}
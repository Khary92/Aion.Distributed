using Core.Server.Tracing.Tracing.Tracers.NoteType.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.NoteType;

public interface INoteTypeUseCaseSelector
{
    ICreateNoteTypeTraceCollector Create { get; }
    IChangeNoteTypeColorTraceCollector ChangeColor { get; }
    IChangeNoteTypeNameTraceCollector ChangeName { get; }
}
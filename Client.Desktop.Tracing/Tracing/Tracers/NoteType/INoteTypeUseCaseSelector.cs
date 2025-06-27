using Client.Desktop.Tracing.Tracing.Tracers.NoteType.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.NoteType;

public interface INoteTypeUseCaseSelector
{
    ICreateNoteTypeTraceCollector Create { get; }
    IChangeNoteTypeColorTraceCollector ChangeColor { get; }
    IChangeNoteTypeNameTraceCollector ChangeName { get; }
}
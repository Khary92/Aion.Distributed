using Client.Tracing.Tracing.Tracers.NoteType.UseCase;
using Service.Admin.Tracing.Tracing.NoteType.UseCase;

namespace Client.Tracing.Tracing.Tracers.NoteType;

public interface INoteTypeUseCaseSelector
{
    ICreateNoteTypeTraceCollector Create { get; }
    IChangeNoteTypeColorTraceCollector ChangeColor { get; }
    IChangeNoteTypeNameTraceCollector ChangeName { get; }
}
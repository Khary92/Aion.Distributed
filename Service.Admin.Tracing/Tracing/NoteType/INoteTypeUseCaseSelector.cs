using Service.Admin.Tracing.Tracing.NoteType.UseCase;

namespace Service.Admin.Tracing.Tracing.NoteType;

public interface INoteTypeUseCaseSelector
{
    ICreateNoteTypeTraceCollector Create { get; }
    IChangeNoteTypeColorTraceCollector ChangeColor { get; }
    IChangeNoteTypeNameTraceCollector ChangeName { get; }
}
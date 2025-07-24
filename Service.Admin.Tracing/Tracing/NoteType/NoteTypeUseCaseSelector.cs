using Service.Admin.Tracing.Tracing.NoteType.UseCase;

namespace Service.Admin.Tracing.Tracing.NoteType;

public class NoteTypeUseCaseSelector(
    ICreateNoteTypeTraceCollector createNoteTypeTraceCollector,
    IChangeNoteTypeColorTraceCollector changeNoteTypeColorTraceCollector,
    IChangeNoteTypeNameTraceCollector changeNoteTypeNameTraceCollector) : INoteTypeUseCaseSelector
{
    public ICreateNoteTypeTraceCollector Create => createNoteTypeTraceCollector;
    public IChangeNoteTypeColorTraceCollector ChangeColor => changeNoteTypeColorTraceCollector;
    public IChangeNoteTypeNameTraceCollector ChangeName => changeNoteTypeNameTraceCollector;
}
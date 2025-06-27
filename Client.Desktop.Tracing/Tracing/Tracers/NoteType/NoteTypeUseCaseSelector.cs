using Client.Desktop.Tracing.Tracing.Tracers.NoteType.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.NoteType;

public class NoteTypeUseCaseSelector(
    ICreateNoteTypeTraceCollector createNoteTypeTraceCollector,
    IChangeNoteTypeColorTraceCollector changeNoteTypeColorTraceCollector,
    IChangeNoteTypeNameTraceCollector changeNoteTypeNameTraceCollector) : INoteTypeUseCaseSelector
{
    public ICreateNoteTypeTraceCollector Create => createNoteTypeTraceCollector;
    public IChangeNoteTypeColorTraceCollector ChangeColor => changeNoteTypeColorTraceCollector;
    public IChangeNoteTypeNameTraceCollector ChangeName => changeNoteTypeNameTraceCollector;
}
using Client.Tracing.Tracing.Tracers.NoteType.UseCase;
using Service.Admin.Tracing.Tracing.NoteType.UseCase;

namespace Client.Tracing.Tracing.Tracers.NoteType;

public class NoteTypeUseCaseSelector(
    ICreateNoteTypeTraceCollector createNoteTypeTraceCollector,
    IChangeNoteTypeColorTraceCollector changeNoteTypeColorTraceCollector,
    IChangeNoteTypeNameTraceCollector changeNoteTypeNameTraceCollector) : INoteTypeUseCaseSelector
{
    public ICreateNoteTypeTraceCollector Create => createNoteTypeTraceCollector;
    public IChangeNoteTypeColorTraceCollector ChangeColor => changeNoteTypeColorTraceCollector;
    public IChangeNoteTypeNameTraceCollector ChangeName => changeNoteTypeNameTraceCollector;
}
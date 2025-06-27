using Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Note;

public class NoteUseCaseSelector(ICreateNoteTraceCollector createNoteTraceCollector, IUpdateNoteTraceCollector updateNoteTraceCollector) : INoteUseCaseSelector
{
    public ICreateNoteTraceCollector Create => createNoteTraceCollector;
    public IUpdateNoteTraceCollector Update => updateNoteTraceCollector;
}
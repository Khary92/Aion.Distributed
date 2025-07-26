using Client.Tracing.Tracing.Tracers.Note.UseCase;

namespace Client.Tracing.Tracing.Tracers.Note;

public class NoteUseCaseSelector(
    ICreateNoteTraceCollector createNoteTraceCollector,
    IUpdateNoteTraceCollector updateNoteTraceCollector) : INoteUseCaseSelector
{
    public ICreateNoteTraceCollector Create => createNoteTraceCollector;
    public IUpdateNoteTraceCollector Update => updateNoteTraceCollector;
}
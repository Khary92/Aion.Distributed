using Client.Desktop.Communication.RequiresChange.Tracers.Note.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Note;

public interface INoteUseCaseSelector
{
    ICreateNoteTraceCollector Create { get; }
    IUpdateNoteTraceCollector Update { get; }
}
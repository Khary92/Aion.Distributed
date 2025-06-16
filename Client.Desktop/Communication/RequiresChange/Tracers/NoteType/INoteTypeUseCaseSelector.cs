using Client.Desktop.Communication.RequiresChange.Tracers.NoteType.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.NoteType;

public interface INoteTypeUseCaseSelector
{
    ICreateNoteTypeTraceCollector Create { get; }
    IChangeNoteTypeColorTraceCollector ChangeColor { get; }
    IChangeNoteTypeNameTraceCollector ChangeName { get; }
}
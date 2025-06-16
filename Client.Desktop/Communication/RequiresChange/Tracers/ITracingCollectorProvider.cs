using Client.Desktop.Communication.RequiresChange.Tracers.AiSettings;
using Client.Desktop.Communication.RequiresChange.Tracers.Export;
using Client.Desktop.Communication.RequiresChange.Tracers.Note;
using Client.Desktop.Communication.RequiresChange.Tracers.NoteType;
using Client.Desktop.Communication.RequiresChange.Tracers.Sprint;
using Client.Desktop.Communication.RequiresChange.Tracers.Tag;
using Client.Desktop.Communication.RequiresChange.Tracers.Ticket;
using Client.Desktop.Communication.RequiresChange.Tracers.TimerSettings;
using Client.Desktop.Communication.RequiresChange.Tracers.WorkDay;

namespace Client.Desktop.Communication.RequiresChange.Tracers;

public interface ITracingCollectorProvider
{
    ITicketUseCaseSelector Ticket { get; }
    INoteTypeUseCaseSelector NoteType { get; }
    ISprintUseCaseSelector Sprint { get; }
    ITagUseCaseSelector Tag { get; }
    INoteUseCaseSelector Note { get; }
    IExportUseCaseSelector Export { get; }
    IWorkDayUseCaseSelector WorkDay { get; }
    IAiSettingsUseCaseSelector AiSettings { get; }
    ITimerSettingsUseCaseSelector TimerSettings { get; }
}
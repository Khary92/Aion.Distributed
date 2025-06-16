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

public class TracingCollectorProvider(
    ITicketUseCaseSelector ticketUseCaseSelector,
    INoteTypeUseCaseSelector noteTypeUseCaseSelector,
    ISprintUseCaseSelector sprintUseCaseSelector,
    ITagUseCaseSelector tagUseCaseSelector,
    INoteUseCaseSelector noteUseCaseSelector,
    IExportUseCaseSelector exportUseCaseSelector,
    IWorkDayUseCaseSelector workDayUseCaseSelector,
    IAiSettingsUseCaseSelector aiSettingsUseCaseSelector,
    ITimerSettingsUseCaseSelector timerSettingsUseCaseSelector) : ITracingCollectorProvider
{
    public ITicketUseCaseSelector Ticket => ticketUseCaseSelector;
    public INoteTypeUseCaseSelector NoteType => noteTypeUseCaseSelector;
    public ISprintUseCaseSelector Sprint => sprintUseCaseSelector;
    public ITagUseCaseSelector Tag => tagUseCaseSelector;
    public INoteUseCaseSelector Note => noteUseCaseSelector;
    public IExportUseCaseSelector Export => exportUseCaseSelector;
    public IWorkDayUseCaseSelector WorkDay => workDayUseCaseSelector;
    public IAiSettingsUseCaseSelector AiSettings => aiSettingsUseCaseSelector;
    public ITimerSettingsUseCaseSelector TimerSettings => timerSettingsUseCaseSelector;
}
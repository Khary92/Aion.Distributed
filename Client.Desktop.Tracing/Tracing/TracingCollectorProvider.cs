using Client.Desktop.Tracing.Tracing.Tracers;
using Client.Desktop.Tracing.Tracing.Tracers.AiSettings;
using Client.Desktop.Tracing.Tracing.Tracers.Export;
using Client.Desktop.Tracing.Tracing.Tracers.Note;
using Client.Desktop.Tracing.Tracing.Tracers.NoteType;
using Client.Desktop.Tracing.Tracing.Tracers.Sprint;
using Client.Desktop.Tracing.Tracing.Tracers.Tag;
using Client.Desktop.Tracing.Tracing.Tracers.Ticket;
using Client.Desktop.Tracing.Tracing.Tracers.TimerSettings;
using Client.Desktop.Tracing.Tracing.Tracers.WorkDay;

namespace Client.Desktop.Tracing.Tracing;

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
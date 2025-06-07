using Contract.Tracing.Tracers.AiSettings;
using Contract.Tracing.Tracers.Export;
using Contract.Tracing.Tracers.Note;
using Contract.Tracing.Tracers.NoteType;
using Contract.Tracing.Tracers.Sprint;
using Contract.Tracing.Tracers.Tag;
using Contract.Tracing.Tracers.Ticket;
using Contract.Tracing.Tracers.TimerSettings;
using Contract.Tracing.Tracers.WorkDay;

namespace Contract.Tracing.Tracers;

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
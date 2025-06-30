using Core.Server.Tracing.Tracing.Tracers.AiSettings;
using Core.Server.Tracing.Tracing.Tracers.Export;
using Core.Server.Tracing.Tracing.Tracers.Note;
using Core.Server.Tracing.Tracing.Tracers.NoteType;
using Core.Server.Tracing.Tracing.Tracers.Sprint;
using Core.Server.Tracing.Tracing.Tracers.Tag;
using Core.Server.Tracing.Tracing.Tracers.Ticket;
using Core.Server.Tracing.Tracing.Tracers.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers.WorkDay;

namespace Core.Server.Tracing.Tracing.Tracers;

public class TraceCollector(
    ITicketUseCaseSelector ticketUseCaseSelector,
    INoteTypeUseCaseSelector noteTypeUseCaseSelector,
    ISprintUseCaseSelector sprintUseCaseSelector,
    ITagUseCaseSelector tagUseCaseSelector,
    INoteUseCaseSelector noteUseCaseSelector,
    IExportUseCaseSelector exportUseCaseSelector,
    IWorkDayUseCaseSelector workDayUseCaseSelector,
    IAiSettingsUseCaseSelector aiSettingsUseCaseSelector,
    ITimerSettingsUseCaseSelector timerSettingsUseCaseSelector) : ITraceCollector
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
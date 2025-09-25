using Core.Server.Tracing.Tracing.Tracers.Client;
using Core.Server.Tracing.Tracing.Tracers.Note;
using Core.Server.Tracing.Tracing.Tracers.NoteType;
using Core.Server.Tracing.Tracing.Tracers.Sprint;
using Core.Server.Tracing.Tracing.Tracers.Statistics;
using Core.Server.Tracing.Tracing.Tracers.Tag;
using Core.Server.Tracing.Tracing.Tracers.Ticket;
using Core.Server.Tracing.Tracing.Tracers.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers.TimeSlot;
using Core.Server.Tracing.Tracing.Tracers.WorkDay;

namespace Core.Server.Tracing.Tracing.Tracers;

public class TraceCollector(
    ITicketUseCaseSelector ticketUseCaseSelector,
    INoteTypeUseCaseSelector noteTypeUseCaseSelector,
    ISprintUseCaseSelector sprintUseCaseSelector,
    ITagUseCaseSelector tagUseCaseSelector,
    INoteUseCaseSelector noteUseCaseSelector,
    ITimerSettingsUseCaseSelector timerSettingsUseCaseSelector,
    IStatisticsDataUseCaseSelector statisticsDataUseCaseSelector,
    ITimeSlotUseCaseSelector timeSlotUseCaseSelector,
    IWorkDayUseCaseSelector workDayUseCaseSelector,
    IClientUseCaseSelector clientUseCaseSelector) : ITraceCollector
{
    public ITicketUseCaseSelector Ticket => ticketUseCaseSelector;
    public INoteTypeUseCaseSelector NoteType => noteTypeUseCaseSelector;
    public ISprintUseCaseSelector Sprint => sprintUseCaseSelector;
    public IStatisticsDataUseCaseSelector Statistics => statisticsDataUseCaseSelector;
    public ITagUseCaseSelector Tag => tagUseCaseSelector;
    public INoteUseCaseSelector Note => noteUseCaseSelector;
    public ITimerSettingsUseCaseSelector TimerSettings => timerSettingsUseCaseSelector;
    public ITimeSlotUseCaseSelector TimeSlot => timeSlotUseCaseSelector;
    public IWorkDayUseCaseSelector WorkDay => workDayUseCaseSelector;
    public IClientUseCaseSelector Client => clientUseCaseSelector;
}
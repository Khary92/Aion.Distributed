using Client.Tracing.Tracing.Tracers.Note;
using Client.Tracing.Tracing.Tracers.NoteType;
using Client.Tracing.Tracing.Tracers.Sprint;
using Client.Tracing.Tracing.Tracers.Statistics;
using Client.Tracing.Tracing.Tracers.Tag;
using Client.Tracing.Tracing.Tracers.Ticket;
using Client.Tracing.Tracing.Tracers.TimeSlot;
using Client.Tracing.Tracing.Tracers.WorkDay;

namespace Client.Tracing.Tracing.Tracers;

public class TraceCollector(
    ITicketUseCaseSelector ticketUseCaseSelector,
    INoteTypeUseCaseSelector noteTypeUseCaseSelector,
    ISprintUseCaseSelector sprintUseCaseSelector,
    ITagUseCaseSelector tagUseCaseSelector,
    INoteUseCaseSelector noteUseCaseSelector,
    IWorkDayUseCaseSelector workDayUseCaseSelector,
    IStatisticsDataUseCaseSelector statisticsDataUseCaseSelector,
    ITimeSlotUseCaseSelector timeSlotUseCaseSelector) : ITraceCollector
{
    public ITicketUseCaseSelector Ticket => ticketUseCaseSelector;
    public INoteTypeUseCaseSelector NoteType => noteTypeUseCaseSelector;
    public ISprintUseCaseSelector Sprint => sprintUseCaseSelector;
    public ITagUseCaseSelector Tag => tagUseCaseSelector;
    public INoteUseCaseSelector Note => noteUseCaseSelector;
    public IWorkDayUseCaseSelector WorkDay => workDayUseCaseSelector;
    public IStatisticsDataUseCaseSelector Statistics => statisticsDataUseCaseSelector;
    public ITimeSlotUseCaseSelector TimeSlot => timeSlotUseCaseSelector;
}
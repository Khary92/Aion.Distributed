using Client.Tracing.Tracing.Tracers.Note;
using Client.Tracing.Tracing.Tracers.NoteType;
using Client.Tracing.Tracing.Tracers.Sprint;
using Client.Tracing.Tracing.Tracers.Statistics;
using Client.Tracing.Tracing.Tracers.Tag;
using Client.Tracing.Tracing.Tracers.Ticket;
using Client.Tracing.Tracing.Tracers.TimeSlot;
using Client.Tracing.Tracing.Tracers.WorkDay;

namespace Client.Tracing.Tracing.Tracers;

public interface ITraceCollector
{
    ITicketUseCaseSelector Ticket { get; }
    INoteTypeUseCaseSelector NoteType { get; }
    ISprintUseCaseSelector Sprint { get; }
    ITagUseCaseSelector Tag { get; }
    INoteUseCaseSelector Note { get; }
    IWorkDayUseCaseSelector WorkDay { get; }
    IStatisticsDataUseCaseSelector Statistics { get; }
    ITimeSlotUseCaseSelector TimeSlot { get; }
}
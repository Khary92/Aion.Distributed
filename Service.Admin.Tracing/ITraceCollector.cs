using Service.Admin.Tracing.Tracing.NoteType;
using Service.Admin.Tracing.Tracing.Sprint;
using Service.Admin.Tracing.Tracing.Tag;
using Service.Admin.Tracing.Tracing.Ticket;
using Service.Admin.Tracing.Tracing.TimerSettings;

namespace Service.Admin.Tracing;

public interface ITraceCollector
{
    ITicketUseCaseSelector Ticket { get; }
    ISprintUseCaseSelector Sprint { get; }
    ITagUseCaseSelector Tag { get; }
    INoteTypeUseCaseSelector NoteType { get; }
    ITimerSettingsUseCaseSelector TimerSettings { get; }
}
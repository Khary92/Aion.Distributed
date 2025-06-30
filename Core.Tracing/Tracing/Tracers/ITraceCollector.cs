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

public interface ITraceCollector
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
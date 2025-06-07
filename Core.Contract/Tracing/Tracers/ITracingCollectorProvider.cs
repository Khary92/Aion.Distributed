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
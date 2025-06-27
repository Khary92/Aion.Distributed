using Client.Desktop.Tracing.Tracing.Tracers.AiSettings;
using Client.Desktop.Tracing.Tracing.Tracers.Export;
using Client.Desktop.Tracing.Tracing.Tracers.Note;
using Client.Desktop.Tracing.Tracing.Tracers.NoteType;
using Client.Desktop.Tracing.Tracing.Tracers.Sprint;
using Client.Desktop.Tracing.Tracing.Tracers.Tag;
using Client.Desktop.Tracing.Tracing.Tracers.Ticket;
using Client.Desktop.Tracing.Tracing.Tracers.TimerSettings;
using Client.Desktop.Tracing.Tracing.Tracers.WorkDay;

namespace Client.Desktop.Tracing.Tracing.Tracers;

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
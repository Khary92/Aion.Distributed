using Core.Server.Tracing.Tracing.Tracers.TimeSlot.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.TimeSlot;

public class TimeSlotUseCaseSelector(
    ISetEndTimeTraceCollector setEndTimeTraceCollector,
    ISetStartTimeTraceCollector startTimeTraceCollector,
    IAddNoteTraceCollector addNoteTraceCollector) : ITimeSlotUseCaseSelector
{
    public ISetEndTimeTraceCollector SetEndTime => setEndTimeTraceCollector;
    public ISetStartTimeTraceCollector SetStartTime => startTimeTraceCollector;
    public IAddNoteTraceCollector AddNote => addNoteTraceCollector;
}
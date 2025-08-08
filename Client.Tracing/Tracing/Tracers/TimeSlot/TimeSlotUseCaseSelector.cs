using Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

namespace Client.Tracing.Tracing.Tracers.TimeSlot;

public class TimeSlotUseCaseSelector(
    ISetEndTimeTraceCollector setEndTimeTraceCollector,
    ISetStartTimeTraceCollector startTimeTraceCollector) : ITimeSlotUseCaseSelector
{
    public ISetEndTimeTraceCollector SetEndTime => setEndTimeTraceCollector;
    public ISetStartTimeTraceCollector SetStartTime => startTimeTraceCollector;
}
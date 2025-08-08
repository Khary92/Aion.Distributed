using Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

namespace Client.Tracing.Tracing.Tracers.TimeSlot;

public interface ITimeSlotUseCaseSelector
{
    ISetEndTimeTraceCollector SetEndTime { get; }
    ISetStartTimeTraceCollector SetStartTime { get; }
}
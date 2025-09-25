using Core.Server.Tracing.Tracing.Tracers.TimeSlot.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.TimeSlot;

public interface ITimeSlotUseCaseSelector
{
    ISetEndTimeTraceCollector SetEndTime { get; }
    ISetStartTimeTraceCollector SetStartTime { get; }
    IAddNoteTraceCollector AddNote { get; }
}
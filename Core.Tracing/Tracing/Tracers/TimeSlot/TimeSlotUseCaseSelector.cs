using Core.Server.Tracing.Tracing.Tracers.TimeSlot.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.TimeSlot;

public class TimeSlotUseCaseSelector(
    ICreateTimeSlotTraceCollector createTimeSlotTraceCollector,
    ISetEndTimeTraceCollector setEndTimeTraceCollector,
    ISetStartTimeTraceCollector startTimeTraceCollector,
    IAddNoteTraceCollector addNoteTraceCollector) : ITimeSlotUseCaseSelector
{
    public ICreateTimeSlotTraceCollector Create => createTimeSlotTraceCollector;
    public ISetEndTimeTraceCollector SetEndTime => setEndTimeTraceCollector;
    public ISetStartTimeTraceCollector SetStartTime => startTimeTraceCollector;
    public IAddNoteTraceCollector AddNote => addNoteTraceCollector;
}
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public class TimeSlotCommandReceiver(ITimeSlotCommandsService timeSlotCommandsService, ITraceCollector tracer)
    : TimeSlotCommandProtoService.TimeSlotCommandProtoServiceBase
{
    public override async Task<CommandResponse> AddNote(AddNoteCommandProto request, ServerCallContext context)
    {
        await tracer.TimeSlot.AddNote.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await timeSlotCommandsService.AddNote(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetStartTime(SetStartTimeCommandProto request,
        ServerCallContext context)
    {
        await tracer.TimeSlot.SetStartTime.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await timeSlotCommandsService.SetStartTime(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetEndTime(SetEndTimeCommandProto request, ServerCallContext context)
    {
        await tracer.TimeSlot.SetEndTime.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await timeSlotCommandsService.SetEndTime(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}
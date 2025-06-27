using System.Threading.Tasks;
using Client.Desktop.Proto;
using Grpc.Net.Client;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public class TimeSlotCommandSender : ITimeSlotCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTimeSlotCommandProto command)
    {
        var response = await _client.CreateTimeSlotAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(AddNoteCommandProto command)
    {
        var response = await _client.AddNoteAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(SetStartTimeCommandProto command)
    {
        var response = await _client.SetStartTimeAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(SetEndTimeCommandProto command)
    {
        var response = await _client.SetEndTimeAsync(command);
        return response.Success;
    }
}
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public class TimeSlotCommandSender : ITimeSlotCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(ClientSetStartTimeCommand command)
    {
        var response = await _client.SetStartTimeAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientSetEndTimeCommand command)
    {
        var response = await _client.SetEndTimeAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientCreateTimeSlotCommand command)
    {
        var response = await _client.CreateTimeSlotAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientAddNoteCommand command)
    {
        var response = await _client.AddNoteAsync(command.ToProto());
        return response.Success;
    }
}
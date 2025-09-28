using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Grpc.Net.Client;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public class TimeSlotCommandSender : ITimeSlotCommandSender
{
    private readonly TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient _client;

    public TimeSlotCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient(channel);
    }

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
}
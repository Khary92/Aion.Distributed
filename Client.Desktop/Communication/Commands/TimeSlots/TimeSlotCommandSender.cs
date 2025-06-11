using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.TimeSlots;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public class TimeSlotCommandSender : ITimeSlotCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimeSlotCommandService.TimeSlotCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTimeSlotCommand command)
    {
        var response = await _client.CreateTimeSlotAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(AddNoteCommand command)
    {
        var response = await _client.AddNoteAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(SetStartTimeCommand command)
    {
        var response = await _client.SetStartTimeAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(SetEndTimeCommand command)
    {
        var response = await _client.SetEndTimeAsync(command);
        return response.Success;
    }
}
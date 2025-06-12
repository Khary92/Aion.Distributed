using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.UseCases;
using Proto.Shared;
using UseCases_CreateTimeSlotControlCommand = Proto.Command.UseCases.CreateTimeSlotControlCommand;
using UseCases_LoadTimeSlotControlCommand = Proto.Command.UseCases.LoadTimeSlotControlCommand;

namespace Client.Desktop.Communication.Commands.UseCases;

public class UseCaseCommandSender : IUseCaseCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseCommandService.UseCaseCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(UseCases_CreateTimeSlotControlCommand command)
    {
        var response = await _client.CreateTimeSlotControlAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UseCases_LoadTimeSlotControlCommand command)
    {
        var response = await _client.LoadTimeSlotControlAsync(command);
        return response.Success;
    }
}
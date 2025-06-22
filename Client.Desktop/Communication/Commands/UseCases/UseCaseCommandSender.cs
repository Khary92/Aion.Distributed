using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.UseCases;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.UseCases;

public class UseCaseCommandSender : IUseCaseCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseCommandProtoService.UseCaseCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTimeSlotControlCommandProto command)
    {
        var response = await _client.CreateTimeSlotControlAsync(command);
        return response.Success;
    }
}
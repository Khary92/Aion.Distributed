using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.UseCases.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.UseCases;

namespace Client.Desktop.Communication.Commands.UseCases;

public class UseCaseCommandSender : IUseCaseCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseCommandProtoService.UseCaseCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(ClientCreateTimeSlotControlCommand command)
    {
        var response = await _client.CreateTimeSlotControlAsync(command.ToProto());
        return response.Success;
    }
}
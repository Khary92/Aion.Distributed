using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Sprints;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.Sprints;

public class SprintCommandSender : ISprintCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly SprintCommandProtoService.SprintCommandProtoServiceClient _client = new(Channel);
    
    public async Task<bool> Send(CreateSprintCommandProto command)
    {
        var response = await _client.CreateSprintAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        var response = await _client.AddTicketToActiveSprintAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(AddTicketToSprintCommandProto command)
    {
        var response = await _client.AddTicketToSprintAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        var response = await _client.SetSprintActiveStatusAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        var response = await _client.UpdateSprintDataAsync(command);
        return response.Success;
    }
}
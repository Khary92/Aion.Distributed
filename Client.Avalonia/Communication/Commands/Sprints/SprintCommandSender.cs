using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Sprints;
using Proto.Shared;

namespace Client.Avalonia.Communication.Commands.Sprints;

public class SprintCommandSender : ISprintCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly SprintsCommandService.SprintsCommandServiceClient _client = new(Channel);
    
    public async Task<bool> Send(CreateSprintCommand command)
    {
        var response = await _client.CreateSprintAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommand command)
    {
        var response = await _client.AddTicketToActiveSprintAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(AddTicketToSprintCommand command)
    {
        var response = await _client.AddTicketToSprintAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(SetSprintActiveStatusCommand command)
    {
        var response = await _client.SetSprintActiveStatusAsync(command);
        return response.Success;
    }
    
    public async Task<bool> Send(UpdateSprintDataCommand command)
    {
        var response = await _client.UpdateSprintDataAsync(command);
        return response.Success;
    }
}
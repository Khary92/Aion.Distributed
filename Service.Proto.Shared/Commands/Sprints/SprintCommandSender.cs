using Grpc.Net.Client;
using Proto.Command.Sprints;

namespace Service.Proto.Shared.Commands.Sprints;

public class SprintCommandSender : ISprintCommandSender
{
    private readonly SprintCommandProtoService.SprintCommandProtoServiceClient _client;
    
    public SprintCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new(channel);
    }
    
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
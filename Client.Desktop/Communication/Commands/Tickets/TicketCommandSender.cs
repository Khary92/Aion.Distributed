using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Proto.Command.Tickets;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.Tickets;

public class TicketCommandSender : ITicketCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TicketCommandService.TicketCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTicketCommand command)
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
        {
            HttpHandler = httpHandler,
            LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole())
        });
        
        try
        {
            var client = new TicketCommandService.TicketCommandServiceClient(channel);
            var response = await client.CreateTicketAsync(command);
            return response.Success;
        }
        catch (RpcException ex)
        {
            // Logge ex.Status, ex.Message etc.
            Console.WriteLine($"gRPC call failed: {ex.Status} - {ex.Message}");
            return false;
        }

    }

    public async Task<bool> Send(UpdateTicketDataCommand command)
    {
        var response = await _client.UpdateTicketDataAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommand command)
    {
        var response = await _client.UpdateTicketDocumentationAsync(command);
        return response.Success;
    }
}
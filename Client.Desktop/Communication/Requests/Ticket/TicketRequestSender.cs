using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests.Ticket;

public class TicketRequestSender(string address, ITokenService tokenService) : ITicketRequestSender, IInitializeAsync
{
    private TicketProtoRequestService.TicketProtoRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TicketProtoRequestService.TicketProtoRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<List<TicketClientModel>> Send(GetAllTicketsRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TicketRequestSender is not initialized");

        var allTicketsAsync = await _client.GetAllTicketsAsync(request);
        return allTicketsAsync.ToClientModelList();
    }

    public async Task<List<TicketClientModel>> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TicketRequestSender is not initialized");

        var ticketsForCurrentSprintAsync = await _client.GetTicketsForCurrentSprintAsync(request);
        return ticketsForCurrentSprintAsync.ToClientModelList();
    }

    public async Task<List<TicketClientModel>> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TicketRequestSender is not initialized");

        var ticketsWithShowAllSwitchAsync = await _client.GetTicketsWithShowAllSwitchAsync(request);
        return ticketsWithShowAllSwitchAsync.ToClientModelList();
    }

    public async Task<TicketClientModel> Send(GetTicketByIdRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TicketRequestSender is not initialized");

        var ticketByIdAsync = await _client.GetTicketByIdAsync(request);
        return ticketByIdAsync.ToClientModel();
    }
}
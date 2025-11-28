using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public class NotesRequestSender(string address, ITokenService tokenService) : INotesRequestSender, IInitializeAsync
{
    private NotesRequestService.NotesRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new NotesRequestService.NotesRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTicketIdRequest request)
    {
        if (_client is null) throw new InvalidOperationException("NotesRequestSender is not initialized");

        var response = await _client.GetNotesByTicketIdAsync(request.ToProto());
        return response.ToClientModelList();
    }

    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTimeSlotIdRequest request)
    {
        if (_client is null) throw new InvalidOperationException("NotesRequestSender is not initialized");

        var response = await _client.GetNotesByTimeSlotIdAsync(request.ToProto());
        return response.ToClientModelList();
    }
}
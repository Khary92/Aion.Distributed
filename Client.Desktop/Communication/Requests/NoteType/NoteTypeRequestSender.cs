using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteType;

public class NoteTypeRequestSender(string address, ITokenService tokenService)
    : INoteTypeRequestSender, IInitializeAsync
{
    private NoteTypeProtoRequestService.NoteTypeProtoRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new NoteTypeProtoRequestService.NoteTypeProtoRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }


    public async Task<List<NoteTypeClientModel>> Send(GetAllNoteTypesRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("NoteTypeRequestSender is not initialized");

        var getAllNoteTypesResponseProto = await _client.GetAllNoteTypesAsync(request);
        return getAllNoteTypesResponseProto.ToClientModelList();
    }

    public async Task<NoteTypeClientModel> Send(GetNoteTypeByIdRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("NoteTypeRequestSender is not initialized");

        var noteTypeByIdAsync = await _client.GetNoteTypeByIdAsync(request);
        return noteTypeByIdAsync.ToClientModel();
    }
}
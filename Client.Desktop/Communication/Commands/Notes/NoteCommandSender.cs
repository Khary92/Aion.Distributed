using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public class NoteCommandSender(string address, ITokenService tokenService) : INoteCommandSender, IInitializeAsync
{
    private NoteCommandProtoService.NoteCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new NoteCommandProtoService.NoteCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ClientCreateNoteCommand command)
    {
        if (_client is null) throw new InvalidOperationException("NoteCommandSender is not initialized");

        var response = await _client.CreateNoteAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientUpdateNoteCommand command)
    {
        if (_client is null) throw new InvalidOperationException("NoteCommandSender is not initialized");

        var response = await _client.UpdateNoteAsync(command.ToProto());
        return response.Success;
    }
}
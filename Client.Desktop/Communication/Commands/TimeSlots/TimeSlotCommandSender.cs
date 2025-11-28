using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public class TimeSlotCommandSender(string address, ITokenService tokenService)
    : ITimeSlotCommandSender, IInitializeAsync
{
    private TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));

        _client = new TimeSlotCommandProtoService.TimeSlotCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ClientSetStartTimeCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("TimeSlotCommandSender is not initialized");

        var response = await _client.SetStartTimeAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientSetEndTimeCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("TimeSlotCommandSender is not initialized");

        var response = await _client.SetEndTimeAsync(command.ToProto());
        return response.Success;
    }
}
using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands.WorkDays;

public class WorkDayCommandSender(string address, ITokenService tokenService) : IWorkDayCommandSender, IInitializeAsync
{
    private WorkDayCommandProtoService.WorkDayCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new WorkDayCommandProtoService.WorkDayCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("WorkDayCommandSender is not initialized");

        var response = await _client.CreateWorkDayAsync(command.ToProto());
        return response.Success;
    }
}
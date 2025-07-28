using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public class WorkDayRequestSender : IWorkDayRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly WorkDayRequestService.WorkDayRequestServiceClient _client = new(Channel);

    public async Task<List<WorkDayClientModel>> Send(ClientGetAllWorkDaysRequest request)
    {
        var response = await _client.GetAllWorkDaysAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModelList();
    }

    public async Task<WorkDayClientModel> Send(ClientGetSelectedWorkDayRequest request)
    {
        var response = await _client.GetSelectedWorkDayAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<WorkDayClientModel> Send(ClientGetWorkDayByDateRequest request)
    {
        var response = await _client.GetWorkDayByDateAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<bool> Send(ClientIsWorkDayExistingRequest request)
    {
        var response = await _client.IsWorkDayExistingAsync(request.ToProto());
        return response.Exists;
    }
}
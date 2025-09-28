using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public class TimeSlotRequestSender : ITimeSlotRequestSender
{
    private readonly TimeSlotRequestService.TimeSlotRequestServiceClient _client;

    public TimeSlotRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TimeSlotRequestService.TimeSlotRequestServiceClient(channel);
    }
    
    public async Task<TimeSlotClientModel> Send(ClientGetTimeSlotByIdRequest request)
    {
        var response = await _client.GetTimeSlotByIdAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<List<TimeSlotClientModel>> Send(ClientGetTimeSlotsForWorkDayIdRequest request)
    {
        var response = await _client.GetTimeSlotsForWorkDayIdAsync(request.ToProto());
        return response == null ? [] : response.ToClientModelList();
    }
}
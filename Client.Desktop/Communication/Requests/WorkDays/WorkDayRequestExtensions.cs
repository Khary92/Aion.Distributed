using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.TimerSettings;
using Proto.DTO.TraceData;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public static class WorkDayRequestExtensions
{
    public static GetAllWorkDaysRequestProto ToProto(this ClientGetAllWorkDaysRequest request) => new()
    {
        TraceData = new TraceDataProto
        {
            TraceId = request.TraceId.ToString()
        }
    };

    public static GetSelectedWorkDayRequestProto ToProto(this ClientGetSelectedWorkDayRequest request) => new()
    {
        WorkDayId = request.WorkDayId.ToString(),
        TraceData = new TraceDataProto
        {
            TraceId = request.TraceId.ToString()
        }
    };

    public static GetWorkDayByDateRequestProto ToProto(this ClientGetWorkDayByDateRequest request) => new()
    {
        Date = Timestamp.FromDateTimeOffset(request.Date),
        TraceData = new TraceDataProto
        {
            TraceId = request.TraceId.ToString()
        }
    };

    public static IsWorkDayExistingRequestProto ToProto(this ClientIsWorkDayExistingRequest request) => new()
    {
        Date = Timestamp.FromDateTimeOffset(request.Date),
        TraceData = new TraceDataProto
        {
            TraceId = request.TraceId.ToString()
        }
    };

    public static WorkDayClientModel ToClientModel(this WorkDayProto proto)
    {
        return new WorkDayClientModel(Guid.Parse(proto.WorkDayId), proto.Date.ToDateTimeOffset());
    }

    public static List<WorkDayClientModel> ToClientModelList(this WorkDayListProto proto)
    {
        return proto.WorkDays.Select(ToClientModel).ToList();
    }
}
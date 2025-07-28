using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.TimerSettings;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public static class WorkDayRequestExtensions
{
    public static GetAllWorkDaysRequestProto ToProto(this ClientGetAllWorkDaysRequest request) => new();

    public static GetSelectedWorkDayRequestProto ToProto(this ClientGetSelectedWorkDayRequest request) => new()
    {
        WorkDayId = request.WorkDayId.ToString()
    };

    public static GetWorkDayByDateRequestProto ToProto(this ClientGetWorkDayByDateRequest request) => new()
    {
        Date = Timestamp.FromDateTimeOffset(request.Date)
    };

    public static IsWorkDayExistingRequestProto ToProto(this ClientIsWorkDayExistingRequest request) => new()
    {
        Date = Timestamp.FromDateTimeOffset(request.Date)
    };

    public static WorkDayClientModel ToClientModel(this WorkDayProto proto) =>
        new(Guid.Parse(proto.WorkDayId), proto.Date.ToDateTimeOffset());

    public static List<WorkDayClientModel> ToClientModelList(this WorkDayListProto proto) =>
        proto.WorkDays.Select(ToClientModel).ToList();
}
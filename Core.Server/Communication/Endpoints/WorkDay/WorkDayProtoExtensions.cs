using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.WorkDays;
using Proto.DTO.TimerSettings;
using Proto.Notifications.WorkDay;
using Proto.Requests.WorkDays;

namespace Core.Server.Communication.Endpoints.WorkDay;

public static class WorkDayProtoExtensions
{
    public static CreateWorkDayCommand ToCommand(this CreateWorkDayCommandProto proto) => new(
        Guid.Parse(proto.WorkDayId), proto.Date.ToDateTimeOffset(), Guid.Parse(proto.TraceData.TraceId));

    public static WorkDayNotification ToNotification(this CreateWorkDayCommand proto) => new()
    {
        WorkDayCreated = new WorkDayCreatedNotification
        {
            WorkDayId = proto.WorkDayId.ToString(),
            Date = proto.Date.ToTimestamp(),
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static WorkDayProto ToProto(this Domain.Entities.WorkDay workDay) => new()
    {
        WorkDayId = workDay.WorkDayId.ToString(),
        Date = workDay.Date.ToTimestamp()
    };

    public static WorkDayListProto ToProtoList(this List<Domain.Entities.WorkDay> workDays)
    {
        var workDayProtos = new WorkDayListProto();

        foreach (var workDay in workDays) workDayProtos.WorkDays.Add(workDay.ToProto());

        return workDayProtos;
    }
}
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.Requests.WorkDays;

public class WorkDayRequestServiceImpl : WorkDayRequestService.WorkDayRequestServiceBase
{
    public override Task<WorkDayListProto> GetAllWorkDays(GetAllWorkDaysRequestProto request, ServerCallContext context)
    {
        var list = new WorkDayListProto();
        list.WorkDays.Add(new WorkDayProto
        {
            WorkDayId = "workday-1",
            Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-2).ToUniversalTime())
        });
        list.WorkDays.Add(new WorkDayProto
        {
            WorkDayId = "workday-2",
            Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-1).ToUniversalTime())
        });
        return Task.FromResult(list);
    }

    public override Task<WorkDayProto> GetSelectedWorkDay(GetSelectedWorkDayRequestProto request,
        ServerCallContext context)
    {
        var selected = new WorkDayProto
        {
            WorkDayId = "workday-2",
            Date = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
        };
        return Task.FromResult(selected);
    }

    public override Task<WorkDayProto> GetWorkDayByDate(GetWorkDayByDateRequestProto request, ServerCallContext context)
    {
        var workDay = new WorkDayProto
        {
            WorkDayId = "workday-by-date",
            Date = request.Date
        };
        return Task.FromResult(workDay);
    }
}
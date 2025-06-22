using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.DTO.TimerSettings;
using Proto.Requests.WorkDays;

namespace Core.Server.Communication.Mocks.WorkDay;

public class MockWorkDayRequestService : WorkDayRequestService.WorkDayRequestServiceBase
{
    public override Task<WorkDayListProto> GetAllWorkDays(GetAllWorkDaysRequestProto request, ServerCallContext context)
    {
        var list = new WorkDayListProto();
        list.WorkDays.Add(new WorkDayProto
        {
            WorkDayId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-2).ToUniversalTime())
        });
        list.WorkDays.Add(new WorkDayProto
        {
            WorkDayId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-1).ToUniversalTime())
        });
        return Task.FromResult(list);
    }

    public override Task<WorkDayProto> GetSelectedWorkDay(GetSelectedWorkDayRequestProto request,
        ServerCallContext context)
    {
        var selected = new WorkDayProto
        {
            WorkDayId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
        };
        return Task.FromResult(selected);
    }

    public override Task<WorkDayProto> GetWorkDayByDate(GetWorkDayByDateRequestProto request, ServerCallContext context)
    {
        var workDay = new WorkDayProto
        {
            WorkDayId = Guid.NewGuid().ToString(),
            Date = request.Date
        };
        return Task.FromResult(workDay);
    }
}
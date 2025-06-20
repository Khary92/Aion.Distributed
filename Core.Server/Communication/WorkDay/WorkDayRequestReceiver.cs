using Grpc.Core;
using Proto.DTO.TimerSettings;
using Proto.Requests.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Communication.WorkDay;

public class WorkDayRequestReceiver(IWorkDayRequestsService workDayRequestsService)
    : WorkDayRequestService.WorkDayRequestServiceBase
{
    public override async Task<WorkDayListProto> GetAllWorkDays(GetAllWorkDaysRequestProto request,
        ServerCallContext context)
    {
        var workDays = await workDayRequestsService.GetAll();
        return workDays.ToProtoList();
    }

    public override async Task<WorkDayProto?> GetSelectedWorkDay(GetSelectedWorkDayRequestProto request,
        ServerCallContext context)
    {
        var workDay = await workDayRequestsService.GetById(Guid.Parse(request.WorkDayId));
        return workDay?.ToProto();
    }

    public override async Task<WorkDayProto?> GetWorkDayByDate(GetWorkDayByDateRequestProto request, ServerCallContext context)
    {
        var workDay = await workDayRequestsService.GetWorkDayByDate(request.Date.ToDateTimeOffset());
        return workDay?.ToProto();
    }
}
using Core.Server.Services.Entities.WorkDays;
using Grpc.Core;
using Proto.DTO.TimerSettings;
using Proto.Requests.WorkDays;

namespace Core.Server.Communication.Endpoints.WorkDay;

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

    public override async Task<WorkDayProto?> GetWorkDayByDate(GetWorkDayByDateRequestProto request,
        ServerCallContext context)
    {
        var workDay = await workDayRequestsService.GetWorkDayByDate(request.Date.ToDateTimeOffset());
        return workDay == null ? new WorkDayProto() : workDay.ToProto();
    }
    
    public override async Task<WorkDayExistsResponseProto> IsWorkDayExisting(IsWorkDayExistingRequestProto request,
        ServerCallContext context)
    {
        return new WorkDayExistsResponseProto
        {
            Exists = await workDayRequestsService.IsWorkDayExisting(request.Date.ToDateTimeOffset())
        };
    }
}
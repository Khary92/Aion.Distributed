using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Google.Protobuf.WellKnownTypes;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public interface IWorkDayRequestSender
{
    Task<List<WorkDayDto>> Send(GetAllWorkDaysRequestProto request);
    Task<WorkDayDto> Send(GetSelectedWorkDayRequestProto request);
    Task<WorkDayDto> Send(GetWorkDayByDateRequestProto request);
}
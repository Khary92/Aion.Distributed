using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;

namespace Client.Avalonia.Communication.Requests.WorkDays;

public interface IWorkDayRequestSender
{
    Task<List<WorkDayDto>> GetAllWorkDays();
    Task<WorkDayDto> GetSelectedWorkDay();
    Task<WorkDayDto> GetWorkDayByDate(Timestamp date);
}
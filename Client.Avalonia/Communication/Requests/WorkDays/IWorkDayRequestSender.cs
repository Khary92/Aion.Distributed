using System.Threading.Tasks;
using Proto.Requests.WorkDays;

namespace Client.Avalonia.Communication.Requests.WorkDays;

public interface IWorkDayRequestSender
{
    Task<WorkDayListProto> GetAllWorkDays();
    Task<WorkDayProto> GetSelectedWorkDay();
    Task<WorkDayProto> GetWorkDayByDate(Google.Protobuf.WellKnownTypes.Timestamp date);
}
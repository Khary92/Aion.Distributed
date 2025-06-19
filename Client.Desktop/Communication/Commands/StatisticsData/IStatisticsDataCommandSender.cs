using System.Threading.Tasks;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public interface IStatisticsDataCommandSender
{
    Task<bool> Send(CreateStatisticsDataCommandProto command);
    Task<bool> Send(ChangeTagSelectionCommandProto command);
    Task<bool> Send(ChangeProductivityCommandProto command);
}
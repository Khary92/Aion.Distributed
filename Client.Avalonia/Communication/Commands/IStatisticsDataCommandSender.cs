using System.Threading.Tasks;
using Proto.Command.StatisticsData;

namespace Client.Avalonia.Communication.Sender;

public interface IStatisticsDataCommandSender
{
    Task<bool> Send(CreateStatisticsDataCommand command);
    Task<bool> Send(ChangeTagSelectionCommand command);
    Task<bool> Send(ChangeProductivityCommand command);
}
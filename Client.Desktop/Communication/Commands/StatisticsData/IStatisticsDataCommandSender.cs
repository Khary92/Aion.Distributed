using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.StatisticsData.Records;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public interface IStatisticsDataCommandSender
{
    Task<bool> Send(ClientCreateStatisticsDataCommand command);
    Task<bool> Send(ClientChangeTagSelectionCommand command);
    Task<bool> Send(ClientChangeProductivityCommand command);
}
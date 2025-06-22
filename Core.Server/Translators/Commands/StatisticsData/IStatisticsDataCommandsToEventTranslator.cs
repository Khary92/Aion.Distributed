using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Domain.Events.StatisticsData;

namespace Core.Server.Translators.Commands.StatisticsData;

public interface IStatisticsDataCommandsToEventTranslator
{
    StatisticsDataEvent ToEvent(CreateStatisticsDataCommand createStatisticsDataCommand);
    StatisticsDataEvent ToEvent(ChangeProductivityCommand changeProductivityCommand);
    StatisticsDataEvent ToEvent(ChangeTagSelectionCommand changeTagSelectionCommand);
}
using Core.Server.Communication.CQRS.Commands.Entities.StatisticsData;
using Domain.Events.StatisticsData;

namespace Core.Server.Translators.StatisticsData;

public interface IStatisticsDataCommandsToEventTranslator
{
    StatisticsDataEvent ToEvent(CreateStatisticsDataCommand createStatisticsDataCommand);
    StatisticsDataEvent ToEvent(ChangeProductivityCommand changeProductivityCommand);
    StatisticsDataEvent ToEvent(ChangeTagSelectionCommand changeTagSelectionCommand);
}
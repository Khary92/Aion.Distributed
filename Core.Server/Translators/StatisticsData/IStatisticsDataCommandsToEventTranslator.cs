using Domain.Events.StatisticsData;
using Service.Server.CQRS.Commands.Entities.StatisticsData;

namespace Service.Server.Old.Translators.StatisticsData;

public interface IStatisticsDataCommandsToEventTranslator
{
    StatisticsDataEvent ToEvent(CreateStatisticsDataCommand createStatisticsDataCommand);
    StatisticsDataEvent ToEvent(ChangeProductivityCommand changeProductivityCommand);
    StatisticsDataEvent ToEvent(ChangeTagSelectionCommand changeTagSelectionCommand);
}
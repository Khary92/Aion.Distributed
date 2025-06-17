using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Domain.Events.StatisticsData;

namespace Application.Translators.StatisticsData;

public interface IStatisticsDataCommandsToEventTranslator
{
    StatisticsDataEvent ToEvent(CreateStatisticsDataCommand createStatisticsDataCommand);
    StatisticsDataEvent ToEvent(ChangeProductivityCommand changeProductivityCommand);
    StatisticsDataEvent ToEvent(ChangeTagSelectionCommand changeTagSelectionCommand);
}
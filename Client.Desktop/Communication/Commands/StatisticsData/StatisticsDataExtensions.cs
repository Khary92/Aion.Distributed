using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Proto;
using Proto.Command.StatisticsData;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public static class StatisticsDataExtensions
{
    public static ChangeTagSelectionCommandProto ToProto(this ClientChangeTagSelectionCommand command)
    {
        return new ChangeTagSelectionCommandProto
        {
            StatisticsDataId = command.StatisticsDataId.ToString(),
            SelectedTagIds = { command.SelectedTagIds.ToRepeatedField() },
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static ChangeProductivityCommandProto ToProto(this ClientChangeProductivityCommand command)
    {
        return new ChangeProductivityCommandProto
        {
            StatisticsDataId = command.StatisticsDataId.ToString(),
            IsProductive = command.IsProductive,
            IsNeutral = command.IsNeutral,
            IsUnproductive = command.IsUnproductive,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }
}
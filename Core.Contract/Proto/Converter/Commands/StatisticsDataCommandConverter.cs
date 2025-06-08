using System;
using System.Collections.Generic;
using Contract.CQRS.Commands.Entities.StatisticsData;
using Proto.Command.StatisticsData;

namespace Contract.Converters
{
    public static class StatisticsDataCommandConverter
    {
        public static ChangeProductivityProtoCommand ToProto(this ChangeProductivityCommand command)
            => new()
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive
            };

        public static ChangeTagSelectionProtoCommand ToProto(this ChangeTagSelectionCommand command)
            => new()
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                SelectedTagIds = { command.SelectedTagIds.ConvertAll(g => g.ToString()) }
            };

        public static CreateStatisticsDataProtoCommand ToProto(this CreateStatisticsDataCommand command)
            => new()
            {
                StatisticsDataId = command.StatisticsDataId.ToString(),
                IsProductive = command.IsProductive,
                IsNeutral = command.IsNeutral,
                IsUnproductive = command.IsUnproductive,
                TagIds = { command.TagIds.ConvertAll(g => g.ToString()) },
                TimeSlotId = command.TimeSlotId.ToString()
            };

        public static ChangeProductivityCommand ToDomain(this ChangeProductivityProtoCommand proto)
            => new(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive);

        public static ChangeTagSelectionCommand ToDomain(this ChangeTagSelectionProtoCommand proto)
            => new(Guid.Parse(proto.StatisticsDataId), proto.SelectedTagIds.ConvertAll(Guid.Parse));

        public static CreateStatisticsDataCommand ToDomain(this CreateStatisticsDataProtoCommand proto)
            => new(
                Guid.Parse(proto.StatisticsDataId),
                proto.IsProductive,
                proto.IsNeutral,
                proto.IsUnproductive,
                proto.TagIds.ConvertAll(Guid.Parse),
                Guid.Parse(proto.TimeSlotId)
            );
    }
}

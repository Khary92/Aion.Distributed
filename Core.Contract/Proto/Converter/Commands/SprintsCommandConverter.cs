using Contract.CQRS.Commands.Entities.Sprints;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using System;

namespace Contract.Proto.Converter.Commands
{
    public static class SprintsCommandConverter
    {
        public static AddTicketToActiveSprintProtoCommand ToProto(this AddTicketToActiveSprintCommand command)
            => new()
            {
                TicketId = command.TicketId.ToString()
            };

        public static AddTicketToSprintProtoCommand ToProto(this AddTicketToSprintCommand command)
            => new()
            {
                SprintId = command.SprintId.ToString(),
                TicketId = command.TicketId.ToString()
            };

        public static CreateSprintProtoCommand ToProto(this CreateSprintCommand command)
            => new()
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name ?? string.Empty,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
                IsActive = command.IsActive,
                TicketIds = { command.TicketIds.ConvertAll(g => g.ToString()) }
            };

        public static SetSprintActiveStatusProtoCommand ToProto(this SetSprintActiveStatusCommand command)
            => new()
            {
                SprintId = command.SprintId.ToString(),
                IsActive = command.IsActive
            };

        public static UpdateSprintDataProtoCommand ToProto(this UpdateSprintDataCommand command)
            => new()
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name ?? string.Empty,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime)
            };

        public static AddTicketToActiveSprintCommand ToDomain(this AddTicketToActiveSprintProtoCommand proto)
            => new(Guid.Parse(proto.TicketId));

        public static AddTicketToSprintCommand ToDomain(this AddTicketToSprintProtoCommand proto)
            => new(Guid.Parse(proto.SprintId), Guid.Parse(proto.TicketId));

        public static CreateSprintCommand ToDomain(this CreateSprintProtoCommand proto)
            => new(
                Guid.Parse(proto.SprintId),
                proto.Name,
                proto.StartTime.ToDateTimeOffset(),
                proto.EndTime.ToDateTimeOffset(),
                proto.IsActive,
                proto.TicketIds.Select(Guid.Parse).ToList()
            );

        public static SetSprintActiveStatusCommand ToDomain(this SetSprintActiveStatusProtoCommand proto)
            => new(Guid.Parse(proto.SprintId), proto.IsActive);

        public static UpdateSprintDataCommand ToDomain(this UpdateSprintDataProtoCommand proto)
            => new(
                Guid.Parse(proto.SprintId),
                proto.Name,
                proto.StartTime.ToDateTimeOffset(),
                proto.EndTime.ToDateTimeOffset()
            );

        public static DateTimeOffset ToDateTimeOffset(this Timestamp timestamp)
        {
            var dt = timestamp.ToDateTime();
            return new DateTimeOffset(dt, TimeSpan.Zero);
        }
    }
}

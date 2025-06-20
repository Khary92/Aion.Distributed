using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.DTO.Sprint;
using Proto.Notifications.Sprint;
using Proto.Requests.Sprints;
using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.CQRS.Requests.Sprints;

namespace Service.Server.Communication.Sprint;

public static class SprintProtoExtensions
{
    public static AddTicketToActiveSprintCommand ToCommand(
        this AddTicketToActiveSprintCommandProto proto) =>
        new(Guid.Parse(proto.TicketId));

    public static SprintNotification ToNotification(this AddTicketToActiveSprintCommand command) =>
        new()
        {
            TicketAddedToActiveSprint = new TicketAddedToActiveSprintNotification
            {
                TicketId = command.TicketId.ToString(),
            }
        };

    public static AddTicketToSprintCommand ToCommand(
        this AddTicketToSprintCommandProto proto) =>
        new(Guid.Parse(proto.SprintId), Guid.Parse(proto.TicketId));

    public static SprintNotification ToNotification(this AddTicketToSprintCommand command) =>
        new()
        {
            TicketAddedToSprint = new TicketAddedToSprintNotification()
            {
                TicketId = command.TicketId.ToString(),
                SprintId = command.SprintId.ToString(),
            }
        };

    public static CreateSprintCommand ToCommand(
        this CreateSprintCommandProto proto) =>
        new(Guid.Parse(proto.SprintId), proto.Name, proto.StartTime.ToDateTimeOffset(),
            proto.EndTime.ToDateTimeOffset(), proto.IsActive, proto.TicketIds.ToGuidList());

    public static SprintNotification ToNotification(this CreateSprintCommand command) =>
        new()
        {
            SprintCreated = new SprintCreatedNotification
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
                IsActive = command.IsActive,
                TicketIds = { command.TicketIds.ToRepeatedField() },
            }
        };

    public static SetSprintActiveStatusCommand ToCommand(
        this SetSprintActiveStatusCommandProto proto) =>
        new(Guid.Parse(proto.SprintId), proto.IsActive);


    public static SprintNotification ToNotification(this SetSprintActiveStatusCommand command) =>
        new()
        {
            SprintActiveStatusSet = new SprintActiveStatusSetNotification
            {
                SprintId = command.SprintId.ToString(),
                IsActive = command.IsActive,
            }
        };

    public static UpdateSprintDataCommand ToCommand(
        this UpdateSprintDataCommandProto proto) =>
        new(Guid.Parse(proto.SprintId), proto.Name, proto.StartTime.ToDateTimeOffset(),
            proto.EndTime.ToDateTimeOffset());


    public static SprintNotification ToNotification(this UpdateSprintDataCommand command) =>
        new()
        {
            SprintDataUpdated = new SprintDataUpdatedNotification
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
            }
        };

    
    public static GetActiveSprintRequest ToCommand() =>
        new();

    public static SprintProto ToProto(this Domain.Entities.Sprint sprint) =>
        new()
        {
            SprintId = sprint.SprintId.ToString(),
            Name = sprint.Name,
            Start = Timestamp.FromDateTimeOffset(sprint.StartDate),
            End = Timestamp.FromDateTimeOffset(sprint.EndDate),
            IsActive = sprint.IsActive,
            TicketIds = { sprint.TicketIds.ToRepeatedField() }
        };

    public static SprintListProto ToProtoList(this List<Domain.Entities.Sprint> sprints)
    {
        var sprintProtos = new SprintListProto();

        foreach (var sprint in sprints)
        {
            sprintProtos.Sprints.Add(sprint.ToProto());
        }

        return sprintProtos;
    }
}
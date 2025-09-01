using Client.Proto;
using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.DTO.Sprint;
using Proto.DTO.TraceData;
using Proto.Notifications.Sprint;
using Proto.Requests.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint;

public static class SprintProtoExtensions
{
    public static AddTicketToActiveSprintCommand ToCommand(
        this AddTicketToActiveSprintCommandProto proto)
    {
        return new AddTicketToActiveSprintCommand(Guid.Parse(proto.TicketId),
            Guid.Parse(proto.TraceData.TraceId));
    }

    public static SprintNotification ToNotification(this AddTicketToActiveSprintCommand proto)
    {
        return new SprintNotification
        {
            TicketAddedToActiveSprint = new TicketAddedToActiveSprintNotification
            {
                TicketId = proto.TicketId.ToString(),
                TraceData = new TraceDataProto
                {
                    TraceId = proto.TraceId.ToString()
                }
            }
        };
    }
    
    public static CreateSprintCommand ToCommand(
        this CreateSprintCommandProto proto)
    {
        return new CreateSprintCommand(Guid.Parse(proto.SprintId), proto.Name,
            proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(), proto.IsActive,
            proto.TicketIds.ToGuidList(),
            Guid.Parse(proto.TraceData.TraceId));
    }

    public static SprintNotification ToNotification(this CreateSprintCommand command)
    {
        return new SprintNotification
        {
            SprintCreated = new SprintCreatedNotification
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
                IsActive = command.IsActive,
                TicketIds = { command.TicketIds.ToRepeatedField() },
                TraceData = new TraceDataProto
                {
                    TraceId = command.TraceId.ToString()
                }
            }
        };
    }

    public static SetSprintActiveStatusCommand ToCommand(
        this SetSprintActiveStatusCommandProto proto)
    {
        return new SetSprintActiveStatusCommand(Guid.Parse(proto.SprintId), proto.IsActive,
            Guid.Parse(proto.TraceData.TraceId));
    }


    public static SprintNotification ToNotification(this SetSprintActiveStatusCommand command)
    {
        return new SprintNotification
        {
            SprintActiveStatusSet = new SprintActiveStatusSetNotification
            {
                SprintId = command.SprintId.ToString(),
                IsActive = command.IsActive,
                TraceData = new TraceDataProto
                {
                    TraceId = command.TraceId.ToString()
                }
            }
        };
    }
    
    public static SprintNotification ToNotification(this AddTicketToSprintCommand command)
    {
        return new SprintNotification
        {
            TicketAddedToActiveSprint = new TicketAddedToActiveSprintNotification
            {
                TicketId = command.TicketId.ToString(),
                TraceData = new TraceDataProto
                {
                    TraceId = command.TraceId.ToString()
                }
            }
        };
    }

    public static UpdateSprintDataCommand ToCommand(
        this UpdateSprintDataCommandProto proto)
    {
        return new UpdateSprintDataCommand(Guid.Parse(proto.SprintId), proto.Name,
            proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(), Guid.Parse(proto.TraceData.TraceId));
    }


    public static SprintNotification ToNotification(this UpdateSprintDataCommand command)
    {
        return new SprintNotification
        {
            SprintDataUpdated = new SprintDataUpdatedNotification
            {
                SprintId = command.SprintId.ToString(),
                Name = command.Name,
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
                TraceData = new TraceDataProto
                {
                    TraceId = command.TraceId.ToString()
                }
            }
        };
    }

    public static SprintProto ToProto(this Domain.Entities.Sprint sprint)
    {
        return new SprintProto
        {
            SprintId = sprint.SprintId.ToString(),
            Name = sprint.Name,
            Start = Timestamp.FromDateTimeOffset(sprint.StartDate),
            End = Timestamp.FromDateTimeOffset(sprint.EndDate),
            IsActive = sprint.IsActive,
            TicketIds = { sprint.TicketIds.ToRepeatedField() }
        };
    }

    public static SprintListProto ToProtoList(this List<Domain.Entities.Sprint> sprints)
    {
        var sprintProtos = new SprintListProto();

        foreach (var sprint in sprints) sprintProtos.Sprints.Add(sprint.ToProto());

        return sprintProtos;
    }
}
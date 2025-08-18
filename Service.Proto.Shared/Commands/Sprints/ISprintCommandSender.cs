using Proto.Command.Sprints;

namespace Service.Proto.Shared.Commands.Sprints;

public interface ISprintCommandSender
{
    Task<bool> Send(CreateSprintCommandProto command);
    Task<bool> Send(AddTicketToActiveSprintCommandProto command);
    Task<bool> Send(SetSprintActiveStatusCommandProto command);
    Task<bool> Send(UpdateSprintDataCommandProto command);
}
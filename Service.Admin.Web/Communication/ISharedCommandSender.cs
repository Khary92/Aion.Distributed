using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tickets;

namespace Service.Admin.Web.Communication;

public interface ISharedCommandSender : ITicketCommandSender, ISprintCommandSender
{
    
}
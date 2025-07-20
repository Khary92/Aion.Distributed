using Service.Proto.Shared.Requests.NoteTypes;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Admin.Web.Communication;

public interface ISharedRequestSender : ITicketRequestSender, ISprintRequestSender, ITagRequestSender, INoteTypeRequestSender
{
    
}
using Proto.DTO.Sprint;
using Proto.DTO.Tag;
using Proto.DTO.Ticket;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Admin.Web.Communication;

public class SharedRequestSender(
    ITicketRequestSender ticketRequestSender,
    ISprintRequestSender sprintRequestSender,
    ITagRequestSender tagRequestSender)
    : ISharedRequestSender
{
    public async Task<TicketListProto> Send(GetAllTicketsRequestProto request)
    {
        return await ticketRequestSender.Send(request);
    }

    public async Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        return await ticketRequestSender.Send(request);
    }

    public async Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        return await ticketRequestSender.Send(request);
    }

    public async Task<TicketProto> Send(GetTicketByIdRequestProto request)
    {
        return await ticketRequestSender.Send(request);
    }

    public async Task<SprintProto?> Send(GetActiveSprintRequestProto request)
    {
        return await sprintRequestSender.Send(request);
    }

    public async Task<SprintListProto> Send(GetAllSprintsRequestProto request)
    {
        return await sprintRequestSender.Send(request);
    }

    public async Task<TagListProto> Send(GetAllTagsRequestProto request)
    {
        return await tagRequestSender.Send(request);
    }

    public async Task<TagProto> Send(GetTagByIdRequestProto request)
    {
        return await tagRequestSender.Send(request);
    }

    public async Task<TagListProto> Send(GetTagsByIdsRequestProto request)
    {
        return await tagRequestSender.Send(request);
    }
}
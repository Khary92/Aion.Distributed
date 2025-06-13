using Grpc.Core;
using Proto.Requests.Tickets;

public class TicketRequestServiceImpl : TicketRequestService.TicketRequestServiceBase
{
    public override Task<TicketListProto> GetAllTickets(GetAllTicketsRequestProto request, ServerCallContext context)
    {
        var response = new TicketListProto();
        response.Tickets.Add(new TicketProto
        {
            TicketId = "ticket-1",
            Name = "Feature A",
            BookingNumber = "BN-001",
            Documentation = "Docs for Feature A",
            SprintIds = { "sprint-1", "sprint-2" }
        });
        response.Tickets.Add(new TicketProto
        {
            TicketId = "ticket-2",
            Name = "Bugfix B",
            BookingNumber = "BN-002",
            Documentation = "Docs for Bugfix B",
            SprintIds = { "sprint-2" }
        });

        return Task.FromResult(response);
    }

    public override Task<TicketListProto> GetTicketsForCurrentSprint(GetTicketsForCurrentSprintRequestProto request,
        ServerCallContext context)
    {
        var response = new TicketListProto();
        response.Tickets.Add(new TicketProto
        {
            TicketId = "ticket-2",
            Name = "Bugfix B",
            BookingNumber = "BN-002",
            Documentation = "Docs for Bugfix B",
            SprintIds = { "current-sprint" }
        });

        return Task.FromResult(response);
    }

    public override Task<TicketListProto> GetTicketsWithShowAllSwitch(GetTicketsWithShowAllSwitchRequestProto request,
        ServerCallContext context)
    {
        var response = new TicketListProto();

        if (request.IsShowAll)
        {
            response.Tickets.Add(new TicketProto
            {
                TicketId = "ticket-1",
                Name = "Feature A",
                BookingNumber = "BN-001",
                Documentation = "Docs for Feature A",
                SprintIds = { "sprint-1", "sprint-2" }
            });
            response.Tickets.Add(new TicketProto
            {
                TicketId = "ticket-2",
                Name = "Bugfix B",
                BookingNumber = "BN-002",
                Documentation = "Docs for Bugfix B",
                SprintIds = { "sprint-2" }
            });
        }
        else
        {
            response.Tickets.Add(new TicketProto
            {
                TicketId = "ticket-2",
                Name = "Bugfix B",
                BookingNumber = "BN-002",
                Documentation = "Docs for Bugfix B",
                SprintIds = { "current-sprint" }
            });
        }

        return Task.FromResult(response);
    }
}
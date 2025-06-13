using Grpc.Core;
using Proto.Requests.Tickets;

public class TicketRequestServiceImpl : TicketRequestService.TicketRequestServiceBase
{
    public override Task<TicketListProto> GetAllTickets(GetAllTicketsRequestProto request, ServerCallContext context)
    {
        var response = new TicketListProto();
        response.Tickets.Add(new TicketProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Feature A",
            BookingNumber = "BN-001",
            Documentation = "Docs for Feature A",
            SprintIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
        });
        response.Tickets.Add(new TicketProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Bugfix B",
            BookingNumber = "BN-002",
            Documentation = "Docs for Bugfix B",
            SprintIds = { Guid.NewGuid().ToString() }
        });

        return Task.FromResult(response);
    }

    public override Task<TicketListProto> GetTicketsForCurrentSprint(GetTicketsForCurrentSprintRequestProto request,
        ServerCallContext context)
    {
        var response = new TicketListProto();
        response.Tickets.Add(new TicketProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Bugfix B",
            BookingNumber = "BN-002",
            Documentation = "Docs for Bugfix B",
            SprintIds = { Guid.NewGuid().ToString() }
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
                TicketId = Guid.NewGuid().ToString(),
                Name = "Feature A",
                BookingNumber = "BN-001",
                Documentation = "Docs for Feature A",
                SprintIds = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
            });
            response.Tickets.Add(new TicketProto
            {
                TicketId = Guid.NewGuid().ToString(),
                Name = "Bugfix B",
                BookingNumber = "BN-002",
                Documentation = "Docs for Bugfix B",
                SprintIds = { Guid.NewGuid().ToString() }
            });
        }
        else
        {
            response.Tickets.Add(new TicketProto
            {
                TicketId = Guid.NewGuid().ToString(),
                Name = "Bugfix B",
                BookingNumber = "BN-002",
                Documentation = "Docs for Bugfix B",
                SprintIds = { Guid.NewGuid().ToString() }
            });
        }

        return Task.FromResult(response);
    }
}
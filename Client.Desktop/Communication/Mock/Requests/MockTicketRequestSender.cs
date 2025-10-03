using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;
using Service.Proto.Shared.Requests.Tickets;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTicketRequestSender(MockDataService mockDataService) : ITicketRequestSender
{
    public Task<TicketListProto> Send(GetAllTicketsRequestProto request)
    {
        var result = new TicketListProto
        {
            Tickets = { mockDataService.Tickets.Select(ConvertToProto).ToList() }
        };

        return Task.FromResult(result);
    }

    public Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        var currentSprint = mockDataService.Sprints.FirstOrDefault(s => s.IsActive);

        if (currentSprint == null) return Task.FromResult(new TicketListProto());

        var result = new TicketListProto
        {
            Tickets =
            {
                mockDataService.Tickets.Where(t => currentSprint.TicketIds.Contains(t.TicketId)).Select(ConvertToProto)
                    .ToList()
            }
        };

        return Task.FromResult(result);
    }

    //TODO this needs to be reimplemented. This is a convenience feature for the web frontend. This is not required here.
    public Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        var result = new TicketListProto
        {
            Tickets = { mockDataService.Tickets.Select(ConvertToProto).ToList() }
        };

        return Task.FromResult(result);
    }

    public Task<TicketProto> Send(GetTicketByIdRequestProto request)
    {
        return Task.FromResult(
            ConvertToProto(mockDataService.Tickets.First(t => t.TicketId.ToString() == request.TicketId)));
    }

    private static TicketProto ConvertToProto(TicketClientModel ticketClientModel)
    {
        return new TicketProto
        {
            TicketId = ticketClientModel.TicketId.ToString(),
            Name = ticketClientModel.Name,
            BookingNumber = ticketClientModel.BookingNumber,
            SprintIds = { ticketClientModel.SprintIds.ToRepeatedField() }
        };
    }
}
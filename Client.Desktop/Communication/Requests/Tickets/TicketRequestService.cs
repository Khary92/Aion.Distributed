using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.Tickets;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.Tickets;

public class TicketRequestSender : ITicketRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TicketRequestService.TicketRequestServiceClient _client = new(Channel);

    public async Task<List<TicketDto>> GetAllTickets()
    {
        var request = new GetAllTicketsRequestProto();
        var response = await _client.GetAllTicketsAsync(request);
        return ToDto(response);
    }

    public async Task<List<TicketDto>> GetTicketsForCurrentSprint()
    {
        var request = new GetTicketsForCurrentSprintRequestProto();
        var response = await _client.GetTicketsForCurrentSprintAsync(request);
        return ToDto(response);
    }

    public async Task<List<TicketDto>> GetTicketsWithShowAllSwitch(bool isShowAll)
    {
        var request = new GetTicketsWithShowAllSwitchRequestProto { IsShowAll = isShowAll };
        var response = await _client.GetTicketsWithShowAllSwitchAsync(request);
        return ToDto(response);
    }

    private static List<TicketDto> ToDto(TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        var result = new List<TicketDto>();
        foreach (var ticket in ticketListProto.Tickets)
        {
            var sprintIds = ticket.SprintIds
                .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
                .Where(guid => guid != Guid.Empty)
                .ToList();

            result.Add(new TicketDto(Guid.Parse(ticket.TicketId), ticket.Name, ticket.BookingNumber,
                ticket.Documentation, new Collection<Guid>(sprintIds)));
        }
        return result;
    }
}
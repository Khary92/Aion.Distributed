using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Proto;
using Grpc.Net.Client;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests.Tickets;

public class TicketRequestSender : ITicketRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TicketRequestService.TicketRequestServiceClient _client = new(Channel);

    public async Task<List<TicketDto>> Send(GetAllTicketsRequestProto request)
    {
        var response = await _client.GetAllTicketsAsync(request);
        return MapToDtoList(response);
    }

    public async Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        var response = await _client.GetTicketsForCurrentSprintAsync(request);
        return MapToDtoList(response);
    }

    public async Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        var response = await _client.GetTicketsWithShowAllSwitchAsync(request);
        return MapToDtoList(response);
    }

    public async Task<TicketDto> Send(GetTicketByIdRequestProto request)
    {
        var response = await _client.GetTicketByIdAsync(request);
        return MapToDto(response!);
    }

    private static List<TicketDto> MapToDtoList(TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(MapToDto)
            .ToList();
    }

    private static TicketDto MapToDto(TicketProto ticket)
    {
        var sprintIds = ticket.SprintIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TicketDto(
            Guid.Parse(ticket.TicketId),
            ticket.Name,
            ticket.BookingNumber,
            ticket.Documentation,
            [..sprintIds]
        );
    }
}
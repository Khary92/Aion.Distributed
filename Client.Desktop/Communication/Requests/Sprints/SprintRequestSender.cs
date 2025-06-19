using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Grpc.Net.Client;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.Sprints;

public class SprintRequestSender : ISprintRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly SprintRequestService.SprintRequestServiceClient _client = new(Channel);

    public async Task<SprintDto?> Send(GetActiveSprintRequestProto request)
    {
        var response = await _client.GetActiveSprintAsync(request);
        
        return response == null? null : ToDto(response, true);
    }

    public async Task<List<SprintDto>> Send(GetAllSprintsRequestProto request)
    {
        var response = await _client.GetAllSprintsAsync(request);

        return response.Sprints.Select(sprint => ToDto(sprint, true)).ToList();
    }
    
    private static SprintDto ToDto(SprintProto proto, bool isActive)
    {
        var ticketIds = proto.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintDto(
            Guid.Parse(proto.SprintId),
            proto.Name,
            isActive,
            proto.Start.ToDateTimeOffset(),
            proto.End.ToDateTimeOffset(),
            ticketIds);
    }
}
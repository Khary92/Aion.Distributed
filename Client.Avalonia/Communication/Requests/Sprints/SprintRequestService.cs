using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.Sprints;
using Proto.Shared;

namespace Client.Avalonia.Communication.Requests.Sprints;

public class SprintRequestSender : ISprintRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly SprintRequestService.SprintRequestServiceClient _client = new(Channel);

    public async Task<SprintDto> GetActiveSprint()
    {
        var request = new GetActiveSprintRequestProto();
        var response = await _client.GetActiveSprintAsync(request);
        return ToDto(response, true);
    }

    public async Task<List<SprintDto>> GetAllSprints()
    {
        var request = new GetAllSprintsRequestProto();
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
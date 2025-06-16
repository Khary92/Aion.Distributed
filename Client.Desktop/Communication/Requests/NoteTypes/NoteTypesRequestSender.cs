using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Grpc.Net.Client;
using Proto.Requests.NoteTypes;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.NoteTypes;

public class NoteTypesRequestSender : INoteTypesRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly NoteTypesRequestService.NoteTypesRequestServiceClient _client = new(Channel);

    public async Task<List<NoteTypeDto>> Send(GetAllNoteTypesRequestProto request)
    {
        var response = await _client.GetAllNoteTypesAsync(request);

        return response.NoteTypes.Select(item => new NoteTypeDto(Guid.Parse(item.NoteTypeId), item.Name, item.Color))
            .ToList();
    }

    public async Task<NoteTypeDto> Send(GetNoteTypeByIdRequestProto request)
    {
        var response = await _client.GetNoteTypeByIdAsync(request);
        return new NoteTypeDto(Guid.Parse(response.NoteTypeId), response.Name, response.Color);
    }
}
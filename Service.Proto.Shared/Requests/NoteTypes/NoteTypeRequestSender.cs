﻿using Grpc.Net.Client;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;

namespace Service.Proto.Shared.Requests.NoteTypes;

public class NoteTypeRequestSender : INoteTypeRequestSender
{
    private readonly NoteTypeProtoRequestService.NoteTypeProtoRequestServiceClient _client;
    
    public NoteTypeRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new(channel);
    }
    public async Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request)
    {
        return await _client.GetAllNoteTypesAsync(request);
    }

    public async Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request)
    {
        return await _client.GetNoteTypeByIdAsync(request);
    }
}
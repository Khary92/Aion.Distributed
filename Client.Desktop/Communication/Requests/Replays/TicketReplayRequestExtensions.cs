using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;
using Proto.Requests.TicketReplay;

namespace Client.Desktop.Communication.Requests.Replays;

public static class TicketReplayRequestExtensions
{
    public static GetTicketReplaysByIdRequestProto ToProto(this ClientGetTicketReplaysByIdRequest request)
    {
        return new GetTicketReplaysByIdRequestProto
        {
            NoteTypeId = request.TicketId.ToString()
        };
    }

    public static List<DocumentationReplay> ToReplayList(this GetReplayResponseProto proto)
    {
        return proto.TicketReplays.Select(tr => new DocumentationReplay(tr.DocumentationEntry)).ToList();
    }
}
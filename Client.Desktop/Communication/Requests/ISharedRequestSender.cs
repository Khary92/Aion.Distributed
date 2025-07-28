using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests;

public interface ISharedRequestSender
{
    //Ticket
    Task<List<TicketClientModel>> Send(ClientGetAllTicketsRequest request);
    Task<List<TicketClientModel>> Send(ClientGetTicketsForCurrentSprintRequest request);
    Task<List<TicketClientModel>> Send(ClientGetTicketsWithShowAllSwitchRequest request);
    Task<TicketClientModel> Send(ClientGetTicketByIdRequest request);

    //Sprint
    Task<SprintClientModel?> Send(ClientGetActiveSprintRequest request);
    Task<List<SprintClientModel?>> Send(ClientGetAllSprintsRequest request);

    //Tags
    Task<List<TagClientModel>> Send(ClientGetAllTagsRequest request);
    Task<TagClientModel> Send(ClientGetTagByIdRequest request);
    Task<List<TagClientModel>> Send(ClientGetTagsByIdsRequest request);

    //NoteTypes
    Task<List<NoteTypeClientModel>> Send(ClientGetAllNoteTypesRequest request);
    Task<NoteTypeClientModel> Send(ClientGetNoteTypeByIdRequest request);
}
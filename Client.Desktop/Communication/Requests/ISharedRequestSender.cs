using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Proto.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests;

public interface ISharedRequestSender
{
    //Ticket
    Task<List<TicketClientModel>> Send(GetAllTicketsRequestProto request);
    Task<List<TicketClientModel>> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<List<TicketClientModel>> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketClientModel> Send(GetTicketByIdRequestProto request);

    //Sprint
    Task<SprintClientModel?> Send(GetActiveSprintRequestProto request);
    Task<List<SprintClientModel?>> Send(GetAllSprintsRequestProto request);

    //Tags
    Task<List<TagClientModel>> Send(GetAllTagsRequestProto request);
    Task<TagClientModel> Send(GetTagByIdRequestProto request);
    Task<List<TagClientModel>> Send(GetTagsByIdsRequestProto request);

    //NoteTypes
    Task<List<NoteTypeClientModel>> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeClientModel> Send(GetNoteTypeByIdRequestProto request);
}
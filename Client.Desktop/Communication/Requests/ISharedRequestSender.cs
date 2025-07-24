using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Proto.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests;

public interface ISharedRequestSender
{
    //Ticket
    Task<List<TicketDto>> Send(GetAllTicketsRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketDto> Send(GetTicketByIdRequestProto request);
    
    //Sprint
    Task<SprintDto?> Send(GetActiveSprintRequestProto request);
    Task<List<SprintDto?>> Send(GetAllSprintsRequestProto request);
    
    //Tags
    Task<List<TagDto>> Send(GetAllTagsRequestProto request);
    Task<TagDto> Send(GetTagByIdRequestProto request);
    Task<List<TagDto>> Send(GetTagsByIdsRequestProto request);
    
    //NoteTypes
    Task<List<NoteTypeDto>> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeDto> Send(GetNoteTypeByIdRequestProto request);
    
    //TimerSettings
    Task<TimerSettingsDto> Send(GetTimerSettingsRequestProto request);
    Task<bool> Send(IsTimerSettingExistingRequestProto request);
}
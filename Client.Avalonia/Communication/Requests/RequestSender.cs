using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Requests.AiSettings;
using Client.Avalonia.Communication.Requests.Notes;
using Client.Avalonia.Communication.Requests.NoteTypes;
using Client.Avalonia.Communication.Requests.Settings;
using Client.Avalonia.Communication.Requests.Sprints;
using Client.Avalonia.Communication.Requests.StatisticsData;
using Client.Avalonia.Communication.Requests.Tags;
using Client.Avalonia.Communication.Requests.Tickets;
using Client.Avalonia.Communication.Requests.TimerSettings;
using Client.Avalonia.Communication.Requests.TimeSlots;
using Client.Avalonia.Communication.Requests.WorkDays;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;

namespace Client.Avalonia.Communication.Requests;

public class RequestSender(
    IAiSettingsRequestSender aiSettingsRequestSender,
    INotesRequestSender notesRequestSender,
    INoteTypesRequestSender noteTypesRequestSender,
    ISettingsRequestSender settingsRequestSender,
    ISprintRequestSender sprintRequestSender,
    IStatisticsDataRequestSender statisticsDataRequestSender,
    ITagRequestSender tagRequestSender,
    ITicketRequestSender ticketRequestSender,
    ITimerSettingsRequestSender timerSettingsRequestSender,
    ITimeSlotRequestSender timeSlotRequestSender,
    IWorkDayRequestSender workDayRequestSender) : IRequestSender
{
    public async Task<AiSettingsDto?> Get(string aiSettingsId)
        => await aiSettingsRequestSender.Get(aiSettingsId);

    public async Task<List<NoteDto>> GetNotesByTicketId(Guid ticketId)
        => await notesRequestSender.GetNotesByTicketId(ticketId);

    public async Task<List<NoteDto>> GetNotesByTimeSlotId(string timeSlotId)
        => await notesRequestSender.GetNotesByTimeSlotId(timeSlotId);

    public async Task<List<NoteTypeDto>> GetAllNoteTypes()
        => await noteTypesRequestSender.GetAllNoteTypes();

    public async Task<NoteTypeDto> GetNoteTypeById(Guid noteTypeId)
        => await noteTypesRequestSender.GetNoteTypeById(noteTypeId);

    public async Task<SettingsDto> GetSettings()
        => await settingsRequestSender.GetSettings();

    public async Task<bool> IsExportPathValid()
        => await settingsRequestSender.IsExportPathValid();

    public async Task<SprintDto> GetActiveSprint()
        => await sprintRequestSender.GetActiveSprint();

    public async Task<List<SprintDto>> GetAllSprints()
        => await sprintRequestSender.GetAllSprints();

    public async Task<StatisticsDataDto> GetByTimeSlotId(string timeSlotId)
        => await statisticsDataRequestSender.GetByTimeSlotId(timeSlotId);

    public async Task<List<TagDto>> GetAllTags()
        => await tagRequestSender.GetAllTags();

    public async Task<TagDto> GetTagById(Guid tagId)
        => await tagRequestSender.GetTagById(tagId);

    public async Task<List<TicketDto>> GetAllTickets()
        => await ticketRequestSender.GetAllTickets();

    public async Task<List<TicketDto>> GetTicketsForCurrentSprint()
        => await ticketRequestSender.GetTicketsForCurrentSprint();

    public async Task<List<TicketDto>> GetTicketsWithShowAllSwitch(bool isShowAll)
        => await ticketRequestSender.GetTicketsWithShowAllSwitch(isShowAll);

    public async Task<TimerSettingsDto> GetTimerSettings()
        => await timerSettingsRequestSender.GetTimerSettings();

    public async Task<bool> IsTimerSettingExisting()
        => await timerSettingsRequestSender.IsTimerSettingExisting();

    public async Task<TimeSlotDto> GetTimeSlotById(string timeSlotId)
        => await timeSlotRequestSender.GetTimeSlotById(timeSlotId);

    public async Task<List<TimeSlotDto>> GetTimeSlotsForWorkDayId(string workDayId)
        => await timeSlotRequestSender.GetTimeSlotsForWorkDayId(workDayId);

    public async Task<List<WorkDayDto>> GetAllWorkDays()
        => await workDayRequestSender.GetAllWorkDays();

    public async Task<WorkDayDto> GetSelectedWorkDay()
        => await workDayRequestSender.GetSelectedWorkDay();

    public async Task<WorkDayDto> GetWorkDayByDate(Timestamp date)
        => await workDayRequestSender.GetWorkDayByDate(date);
}
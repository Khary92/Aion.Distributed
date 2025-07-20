using System;
using Newtonsoft.Json;
using Client.Desktop.DTO;

namespace Client.Desktop.Converter;

public static class TracingExtensions
{
    public static string AsTraceAttributes(this TicketDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            ticketId = dto.TicketId.ToString(),
            name = dto.Name,
            bookingNumber = dto.BookingNumber,
            sprintIds = string.Join(",", dto.SprintIds)
        });
    }

    public static string AsTraceAttributes(this NoteTypeDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            ticketId = dto.NoteTypeId.ToString(),
            name = dto.Name,
            color = dto.Color
        });
    }

    public static string AsTraceAttributes(this SprintDto? dto)
    {
        if (dto == null) return string.Empty;
        
        return JsonConvert.SerializeObject(new
        {
            sprintId = dto.SprintId.ToString(),
            name = dto.Name,
            startTime = dto.StartTime.ToString(),
            endTime = dto.EndTime.ToString(),
            isActive = dto.IsActive.ToString(),
            ticketIds = string.Join(",", dto.TicketIds)
        });
    }

    public static string AsTraceAttributes(this TagDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            sprintId = dto.TagId.ToString(),
            name = dto.Name
        });
    }

    public static string AsTraceAttributes(this NoteDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            noteId = dto.NoteId.ToString(),
            noteTypeId = dto.NoteTypeId.ToString(),
            timeSlotId = dto.TimeSlotId.ToString(),
            text = dto.Text,
            timeStamp = dto.TimeStamp.ToString()
        });
    }

    public static string AsTraceAttributes(this WorkDayDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            workDayId = dto.WorkDayId.ToString(),
            date = dto.Date.ToString()
        });
    }

    public static string AsTraceAttributes(this AiSettingsDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            aiSettingsId = dto.AiSettingsId.ToString(),
            languageModelPath = dto.LanguageModelPath,
            prompt = dto.Prompt
        });
    }

    public static string AsTraceAttributes(this TimerSettingsDto dto)
    {
        return JsonConvert.SerializeObject(new
        {
            timerSettingsId = dto.TimerSettingsId.ToString(),
            documentationSaveInterval = dto.DocumentationSaveInterval.ToString(),
            snapshotSaveInterval = dto.SnapshotSaveInterval.ToString()
        });
    }

    public static string AsTypeString(this Type type)
    {
        return type.BaseType!.Name;
    }
}
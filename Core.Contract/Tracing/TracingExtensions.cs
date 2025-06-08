using Contract.DTO;

namespace Contract.Tracing;

public static class TracingExtensions
{
    public static Dictionary<string, string> AsTraceAttributes(this TicketDto dto)
    {
        return new Dictionary<string, string>
        {
            { "ticketId", dto.TicketId.ToString() },
            { "name", dto.Name },
            { "bookingNumber", dto.BookingNumber },
            { "sprintIds", string.Join(",", dto.SprintIds) }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this NoteTypeDto dto)
    {
        return new Dictionary<string, string>
        {
            { "ticketId", dto.NoteTypeId.ToString() },
            { "name", dto.Name },
            { "color", dto.Color }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this SprintDto dto)
    {
        return new Dictionary<string, string>
        {
            { "sprintId", dto.SprintId.ToString() },
            { "name", dto.Name },
            { "startTime", dto.StartTime.ToString() },
            { "endTime", dto.EndTime.ToString() },
            { "isActive", dto.IsActive.ToString() },
            { "ticketIds", string.Join(",", dto.TicketIds) }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this TagDto dto)
    {
        return new Dictionary<string, string>
        {
            { "sprintId", dto.TagId.ToString() },
            { "name", dto.Name }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this NoteDto dto)
    {
        return new Dictionary<string, string>
        {
            { "noteId", dto.NoteId.ToString() },
            { "noteTypeId", dto.NoteTypeId.ToString() },
            { "timeSlotId", dto.TimeSlotId.ToString() },
            { "text", dto.Text },
            { "timeStamp", dto.TimeStamp.ToString() }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this WorkDayDto dto)
    {
        return new Dictionary<string, string>
        {
            { "workDayId", dto.WorkDayId.ToString() },
            { "date", dto.Date.ToString() }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this AiSettingsDto dto)
    {
        return new Dictionary<string, string>
        {
            { "aiSettingsId", dto.AiSettingsId.ToString() },
            { "languageModelPath", dto.LanguageModelPath },
            { "prompt", dto.Prompt }
        };
    }

    public static Dictionary<string, string> AsTraceAttributes(this TimerSettingsDto dto)
    {
        return new Dictionary<string, string>
        {
            { "timerSettingsId", dto.TimerSettingsId.ToString() },
            { "documentationSaveInterval", dto.DocumentationSaveInterval.ToString() },
            { "snapshotSaveInterval", dto.SnapshotSaveInterval.ToString() }
        };
    }
}
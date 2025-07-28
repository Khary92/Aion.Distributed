using Client.Desktop.DataModels;
using Newtonsoft.Json;

namespace Client.Desktop.Presentation;

public static class TracingExtensions
{
    public static string AsTraceAttributes(this TicketClientModel clientModel)
    {
        return JsonConvert.SerializeObject(new
        {
            ticketId = clientModel.TicketId.ToString(),
            name = clientModel.Name,
            bookingNumber = clientModel.BookingNumber,
            sprintIds = string.Join(",", clientModel.SprintIds)
        });
    }

    public static string AsTraceAttributes(this NoteTypeClientModel clientModel)
    {
        return JsonConvert.SerializeObject(new
        {
            ticketId = clientModel.NoteTypeId.ToString(),
            name = clientModel.Name,
            color = clientModel.Color
        });
    }

    public static string AsTraceAttributes(this SprintClientModel? dto)
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

    public static string AsTraceAttributes(this TagClientModel clientModel)
    {
        return JsonConvert.SerializeObject(new
        {
            sprintId = clientModel.TagId.ToString(),
            name = clientModel.Name
        });
    }

    public static string AsTraceAttributes(this NoteClientModel clientModel)
    {
        return JsonConvert.SerializeObject(new
        {
            noteId = clientModel.NoteId.ToString(),
            noteTypeId = clientModel.NoteTypeId.ToString(),
            timeSlotId = clientModel.TimeSlotId.ToString(),
            text = clientModel.Text,
            timeStamp = clientModel.TimeStamp.ToString()
        });
    }

    public static string AsTraceAttributes(this WorkDayClientModel clientModel)
    {
        return JsonConvert.SerializeObject(new
        {
            workDayId = clientModel.WorkDayId.ToString(),
            date = clientModel.Date.ToString()
        });
    }
}
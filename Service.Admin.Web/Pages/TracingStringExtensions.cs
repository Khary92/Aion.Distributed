using Newtonsoft.Json;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Pages;

public static class TracingStringExtensions
{
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
}
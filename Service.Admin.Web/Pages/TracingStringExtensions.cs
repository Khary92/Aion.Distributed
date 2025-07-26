using Newtonsoft.Json;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Pages;

public static class TracingStringExtensions
{
    public static string AsTraceAttributes(this SprintWebModel? dto)
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

    public static string AsTraceAttributes(this TagWebModel webModel)
    {
        return JsonConvert.SerializeObject(new
        {
            sprintId = webModel.TagId.ToString(),
            name = webModel.Name
        });
    }
}
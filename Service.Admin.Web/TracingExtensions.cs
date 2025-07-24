using Newtonsoft.Json;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web;

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

    public static string AsTypeString(this Type type)
    {
        return type.BaseType!.Name;
    }
}
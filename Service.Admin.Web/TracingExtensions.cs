using Newtonsoft.Json;
using Service.Admin.Web.Models;

namespace Service.Admin.Web;

public static class TracingExtensions
{
    public static string AsTraceAttributes(this TicketWebModel webModel)
    {
        return JsonConvert.SerializeObject(new
        {
            ticketId = webModel.TicketId.ToString(),
            name = webModel.Name,
            bookingNumber = webModel.BookingNumber,
            sprintIds = string.Join(",", webModel.SprintIds)
        });
    }

    public static string AsTypeString(this Type type)
    {
        return type.BaseType!.Name;
    }
}
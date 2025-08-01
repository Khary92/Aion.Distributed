using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Wrappers;

public record NewTicketMessage(TicketWebModel Ticket, Guid TraceId);
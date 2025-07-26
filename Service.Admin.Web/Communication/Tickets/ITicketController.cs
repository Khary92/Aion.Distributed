using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets;

public interface ITicketController
{
    TicketWebModel? SelectedTicket { get; set; }
    string NewTicketName { get; set; }
    string NewTicketBookingNumber { get; set; }
    bool IsShowAllTicketsActive { get; set; }
    bool IsEditMode { get; set; }
    Task CreateOrUpdateTicket();
    void SetEditMode();
    Task AddTicketToCurrentSprint();
}
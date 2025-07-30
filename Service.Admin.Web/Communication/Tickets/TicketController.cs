using Proto.Command.Sprints;
using Proto.Command.Tickets;
using Service.Admin.Tracing;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets;

public class TicketController(ITraceCollector tracer, ISharedCommandSender commandSender) : ITicketController
{
    public TicketWebModel? SelectedTicket { get; set; }

    public string NewTicketName { get; set; } = string.Empty;
    public string NewTicketBookingNumber { get; set; } = string.Empty;
    public bool IsShowAllTicketsActive { get; set; }
    public bool IsEditMode { get; set; }

    private async Task UpdateTicket()
    {
        if (SelectedTicket == null) return;
        
        SelectedTicket.Name = NewTicketName;
        SelectedTicket.BookingNumber = NewTicketBookingNumber;
        await commandSender.Send(new UpdateTicketDataCommandProto
        {
            TicketId = SelectedTicket.TicketId.ToString(),
            Name = NewTicketName,
            BookingNumber = NewTicketBookingNumber,
            SprintIds = { SelectedTicket.SprintIds.ToRepeatedField() }
        });
    }

    private async Task CreateTicket()
    {
        var createTicketCommandProto = new CreateTicketCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = NewTicketName,
            BookingNumber = NewTicketBookingNumber,
            TraceData = new()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        // TODO well there is no more ViewModel
        await tracer.Ticket.Create.StartUseCase(GetType(), Guid.Parse(createTicketCommandProto.TicketId),
            createTicketCommandProto);
        await tracer.Ticket.Create.CommandSent(GetType(), Guid.Parse(createTicketCommandProto.TicketId),
            createTicketCommandProto);
        await commandSender.Send(createTicketCommandProto);

        NewTicketName = NewTicketBookingNumber = string.Empty;
        IsEditMode = false;
    }
    
    public Task CreateOrUpdateTicket()
    {
        return IsUpdateRequired() ? UpdateTicket() : CreateTicket();
    }

    private bool IsUpdateRequired()
    {
        return IsEditMode && SelectedTicket != null;
    }

    public void SetEditMode()
    {
        if (SelectedTicket == null) return;

        IsEditMode = !IsEditMode;
        if (IsEditMode)
        {
            NewTicketName = SelectedTicket.Name;
            NewTicketBookingNumber = SelectedTicket.BookingNumber;
            return;
        }

        NewTicketName = NewTicketBookingNumber = "";
    }

    public async Task AddTicketToCurrentSprint()
    {
        if (SelectedTicket == null)
        {
            return;
        }

        await commandSender.Send(new AddTicketToActiveSprintCommandProto
        {
            TicketId = SelectedTicket.TicketId.ToString()
        });
    }
}
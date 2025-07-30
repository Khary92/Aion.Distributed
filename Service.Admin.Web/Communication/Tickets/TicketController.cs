using Proto.Command.Sprints;
using Proto.Command.Tickets;
using Proto.DTO.TraceData;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tickets.Records;
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
            SprintIds = { SelectedTicket.SprintIds.ToRepeatedField() },
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        });
    }

    private async Task CreateTicket()
    {
        var createTicketCommand =
            new WebCreateTicketCommand(Guid.NewGuid(), NewTicketName, NewTicketBookingNumber, [], Guid.NewGuid());

        // TODO well there is no more ViewModel
        await tracer.Ticket.Create.StartUseCase(GetType(), createTicketCommand.TraceId, createTicketCommand);
        await tracer.Ticket.Create.CommandSent(GetType(), createTicketCommand.TraceId, createTicketCommand);
        
        await commandSender.Send(createTicketCommand.ToProto());

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
            TicketId = SelectedTicket.TicketId.ToString(),
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        });
    }
}
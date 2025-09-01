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
    public bool IsEditMode { get; private set; }

    public Task CreateOrUpdateTicket()
    {
        return IsUpdateRequired() ? UpdateTicket() : CreateTicket();
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
        var traceId = Guid.NewGuid();
        await tracer.Sprint.AddTicketToSprint.StartUseCase(GetType(), traceId);

        if (SelectedTicket == null)
        {
            await tracer.Sprint.AddTicketToSprint.NoEntitySelected(GetType(), traceId);
            return;
        }

        var addTicketCommand = new WebAddTicketToSprintCommand(SelectedTicket.TicketId, traceId);

        await tracer.Sprint.AddTicketToSprint.SendingCommand(GetType(), traceId, addTicketCommand);
        await commandSender.Send(addTicketCommand.ToProto());
    }

    private async Task UpdateTicket()
    {
        var traceId = Guid.NewGuid();

        await tracer.Ticket.Update.StartUseCase(GetType(), traceId);

        if (SelectedTicket == null)
        {
            await tracer.Ticket.Update.NoEntitySelected(GetType(), traceId);
            return;
        }

        SelectedTicket.Name = NewTicketName;
        SelectedTicket.BookingNumber = NewTicketBookingNumber;

        var updateTicketCommand =
            new WebUpdateTicketCommand(SelectedTicket.TicketId, NewTicketName, NewTicketBookingNumber,
                SelectedTicket.SprintIds, traceId);

        await tracer.Ticket.Update.SendingCommand(GetType(), updateTicketCommand.TraceId, updateTicketCommand);
        await commandSender.Send(updateTicketCommand.ToProto());
    }

    private async Task CreateTicket()
    {
        var traceId = Guid.NewGuid();

        await tracer.Ticket.Create.StartUseCase(GetType(), traceId);

        var createTicketCommand =
            new WebCreateTicketCommand(Guid.NewGuid(), NewTicketName, NewTicketBookingNumber, [], traceId);

        await tracer.Ticket.Create.SendingCommand(GetType(), createTicketCommand.TraceId, createTicketCommand);
        await commandSender.Send(createTicketCommand.ToProto());

        NewTicketName = NewTicketBookingNumber = string.Empty;
        IsEditMode = false;
    }

    private bool IsUpdateRequired()
    {
        return IsEditMode && SelectedTicket != null;
    }
}
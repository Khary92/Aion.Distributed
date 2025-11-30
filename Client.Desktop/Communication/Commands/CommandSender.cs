using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client;
using Client.Desktop.Communication.Commands.Client.Records;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Policies;
using ITicketCommandSender = Client.Desktop.Communication.Commands.Ticket.ITicketCommandSender;

namespace Client.Desktop.Communication.Commands;

public class CommandSender(
    INoteCommandSender noteCommandSender,
    IStatisticsDataCommandSender statisticsDataCommandSender,
    ITimeSlotCommandSender timeSlotCommandSender,
    IClientCommandSender clientCommandSender,
    IWorkDayCommandSender workDayCommandSender,
    ITicketCommandSender ticketCommandSender,
    CommandRetryPolicy sender) : ICommandSender
{
    public async Task<bool> Send(ClientCreateNoteCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            noteCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientUpdateNoteCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            noteCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientChangeTagSelectionCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            statisticsDataCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientChangeProductivityCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            statisticsDataCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientSetStartTimeCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            timeSlotCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientSetEndTimeCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            timeSlotCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientCreateTrackingControlCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            clientCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            workDayCommandSender.Send(command));
    }

    public async Task<bool> Send(ClientUpdateTicketDocumentationCommand command)
    {
        return await sender.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }
}
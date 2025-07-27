using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.UseCases.Records;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Service.Proto.Shared.Commands.Tickets;

namespace Client.Desktop.Communication.Commands;

public class CommandSender(
    INoteCommandSender noteCommandSender,
    IStatisticsDataCommandSender statisticsDataCommandSender,
    ITimeSlotCommandSender timeSlotCommandSender,
    IUseCaseCommandSender useCaseCommandSender,
    IWorkDayCommandSender workDayCommandSender,
    ITicketCommandSender ticketCommandSender) : ICommandSender
{
    public async Task<bool> Send(ClientCreateNoteCommand command)
    {
        return await noteCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientUpdateNoteCommand command)
    {
        return await noteCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientCreateStatisticsDataCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientChangeTagSelectionCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientChangeProductivityCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientCreateTimeSlotCommand command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientAddNoteCommand command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientSetStartTimeCommand command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientSetEndTimeCommand command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientCreateTimeSlotControlCommand command)
    {
        return await useCaseCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        return await workDayCommandSender.Send(command);
    }

    public async Task<bool> Send(ClientUpdateTicketDocumentationCommand command)
    {
        return await ticketCommandSender.Send(command.ToProto());
    }
}
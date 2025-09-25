using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.WorkDays;

namespace Client.Desktop.Communication.Commands;

public interface ICommandSender :
    INoteCommandSender,
    IStatisticsDataCommandSender,
    ITimeSlotCommandSender,
    IClientCommandSender,
    IWorkDayCommandSender
{
    Task<bool> Send(ClientUpdateTicketDocumentationCommand command);
}
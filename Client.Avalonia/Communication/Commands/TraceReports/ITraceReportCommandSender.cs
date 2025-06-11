using System.Threading.Tasks;
using Proto.Command.TraceReports;

namespace Client.Avalonia.Communication.Commands.TraceReports;

public interface ITraceReportCommandSender
{
    Task<bool> Send(SendTraceReportCommand command);
}
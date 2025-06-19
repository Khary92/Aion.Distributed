using System.Threading.Tasks;
using Proto.Command.TraceReports;

namespace Client.Desktop.Communication.Commands.TraceReports;

public interface ITraceReportCommandSender
{
    Task<bool> Send(SendTraceReportCommandProto command);
}
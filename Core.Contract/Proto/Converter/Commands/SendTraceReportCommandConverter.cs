using Contract.CQRS.Commands.Tracing;
using Proto.Command.Tracing;

namespace Contract.Converters
{
    public static class TraceReportCommandConverter
    {
        public static SendTraceReportProtoCommand ToProto(this SendTraceReportCommand command) => new()
        {
            TraceReportDto = command.TraceReportDto
        };
    }
}
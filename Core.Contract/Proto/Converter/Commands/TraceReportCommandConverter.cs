using Contract.CQRS.Commands.Tracing;
using Proto.Command.Tracing;

namespace Contract.Proto.Converter.Commands
{
    public static class TraceReportCommandConverter
    {
        public static SendTraceReportProtoCommand ToProto(this SendTraceReportCommand command) => new()
        {
            TraceReportDto = command.TraceReportDto
        };
    }
}
using MediatR;

namespace Contract.CQRS.Commands.Tracing;

public record SendTraceReportCommand(string TraceReportDto)
    : IRequest<Unit>;
using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Commands.Tracing;

public record SendTraceReportCommand(TraceReportDto TraceReportDto)
    : IRequest<Unit>;
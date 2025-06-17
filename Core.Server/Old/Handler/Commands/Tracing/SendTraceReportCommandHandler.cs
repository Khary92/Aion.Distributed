using Application.Contract.CQRS.Commands.Tracing;
using Application.Contract.Notifications.UseCase;
using MediatR;

namespace Application.Handler.Commands.Tracing;

public class SendTraceReportCommandHandler(IMediator mediator) :  IRequestHandler<SendTraceReportCommand, Unit>
{
    public async Task<Unit> Handle(SendTraceReportCommand request, CancellationToken cancellationToken)
    {
        await mediator.Publish(new TraceReportSentNotification(request.TraceReportDto), cancellationToken);
        return Unit.Value;
    }
}
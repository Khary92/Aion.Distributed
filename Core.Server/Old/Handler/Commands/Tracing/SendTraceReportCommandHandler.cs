namespace Service.Server.Old.Handler.Commands.Tracing;

public class SendTraceReportCommandHandler(IMediator mediator) :  IRequestHandler<SendTraceReportCommand, Unit>
{
    public async Task<Unit> Handle(SendTraceReportCommand request, CancellationToken cancellationToken)
    {
        await mediator.Publish(new TraceReportSentNotification(request.TraceReportDto), cancellationToken);
        return Unit.Value;
    }
}
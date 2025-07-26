using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Ticket;

public class TicketVerificationProvider : IVerificationProvider
{
    private readonly ImmutableList<VerificationStep> _createTicketSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandSent, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationSent, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.AggregateReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));

    public TraceSinkId TraceSinkId => TraceSinkId.Ticket;

    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        if (useCaseMeta == UseCaseMeta.CreateTicket) return _createTicketSteps;

        return ImmutableList.Create<VerificationStep>();
    }
}
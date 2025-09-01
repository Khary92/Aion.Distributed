using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Steps;

public class TicketVerificationProvider : IVerificationProvider
{
    private readonly ImmutableList<VerificationStep> _createTicketSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));

    public SortingType SortingType => SortingType.Ticket;

    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        return useCaseMeta == UseCaseMeta.CreateTicket ? _createTicketSteps : ImmutableList.Create<VerificationStep>();
    }
}
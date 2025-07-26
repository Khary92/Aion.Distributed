using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketUseCasesSteps
{
    public static ImmutableList<VerificationStep> CreateTicket => ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandSent, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.AggregateReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));
}
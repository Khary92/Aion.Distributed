using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Steps;

public class TicketVerificationProvider : IVerificationProvider
{
    private readonly Dictionary<UseCaseMeta, ImmutableList<VerificationStep>> _verificationProvider = new();

    public TicketVerificationProvider()
    {
        _verificationProvider.Add(UseCaseMeta.CreateTicket, CreateTicketSteps);
        _verificationProvider.Add(UseCaseMeta.UpdateTicket, UpdateTicketSteps);
        _verificationProvider.Add(UseCaseMeta.UpdateTicketDocumentation, UpdateTicketDocumentationSteps);
    }

    private static readonly ImmutableList<VerificationStep> CreateTicketSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));
    
    private static readonly ImmutableList<VerificationStep> UpdateTicketSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.PropertyChanged, Invoked.AtLeast, 1));
    
    private static readonly ImmutableList<VerificationStep> UpdateTicketDocumentationSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.PropertyChanged, Invoked.AtLeast, 1));

    public SortingType SortingType => SortingType.Ticket;

    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        return _verificationProvider.TryGetValue(useCaseMeta, out var value)
            ? value
            : ImmutableList<VerificationStep>.Empty;
    }
}
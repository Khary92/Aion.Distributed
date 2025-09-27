using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Steps;

public class NoteVerificationProvider : IVerificationProvider
{
    private static readonly ImmutableList<VerificationStep> CreateNoteSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));

    private static readonly ImmutableList<VerificationStep> UpdateNoteSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.PropertyChanged, Invoked.AtLeast, 1));

    private readonly Dictionary<UseCaseMeta, ImmutableList<VerificationStep>> _verificationProvider = new();

    public NoteVerificationProvider()
    {
        _verificationProvider.Add(UseCaseMeta.CreateNote, CreateNoteSteps);
        _verificationProvider.Add(UseCaseMeta.UpdateNote, UpdateNoteSteps);
    }

    public SortingType SortingType => SortingType.Note;

    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        return _verificationProvider.TryGetValue(useCaseMeta, out var value)
            ? value
            : ImmutableList<VerificationStep>.Empty;
    }
}
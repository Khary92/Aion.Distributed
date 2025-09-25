using System.Collections.Immutable;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Steps;

public class TimerSettingsVerificationProvider : IVerificationProvider
{
    private readonly Dictionary<UseCaseMeta, ImmutableList<VerificationStep>> _verificationProvider = new();

    public TimerSettingsVerificationProvider()
    {
        _verificationProvider.Add(UseCaseMeta.CreateTimerSettings, CreateTimerSettingsSteps);
        _verificationProvider.Add(UseCaseMeta.ChangeDocuTimerSaveInterval, ChangeDocuTimerSaveIntervalSteps);
        _verificationProvider.Add(UseCaseMeta.ChangeSnapshotSaveInterval, ChangeSnapshotSaveIntervalSteps);
    }

    private static readonly ImmutableList<VerificationStep> CreateTimerSettingsSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.AggregateAdded, Invoked.AtLeast, 1));

    private static readonly ImmutableList<VerificationStep> ChangeDocuTimerSaveIntervalSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.PropertyChanged, Invoked.AtLeast, 1));

    private static readonly ImmutableList<VerificationStep> ChangeSnapshotSaveIntervalSteps = ImmutableList.Create(
        new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.CommandReceived, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.EventPersisted, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.SendingNotification, Invoked.Equals, 1),
        new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
        new VerificationStep(LoggingMeta.PropertyChanged, Invoked.AtLeast, 1));

    public SortingType SortingType => SortingType.TimerSettings;

    public ImmutableList<VerificationStep> GetVerificationSteps(UseCaseMeta useCaseMeta)
    {
        return _verificationProvider.TryGetValue(useCaseMeta, out var value)
            ? value
            : ImmutableList<VerificationStep>.Empty;
    }
}
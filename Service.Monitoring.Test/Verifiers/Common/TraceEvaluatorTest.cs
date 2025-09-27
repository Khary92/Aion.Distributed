using System.Collections.Immutable;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Test.Verifiers.Common;

[TestFixture]
[TestOf(typeof(TraceEvaluator))]
public class TraceEvaluatorTest
{
    private static TraceData Trace(LoggingMeta loggingMeta)
    {
        return new TraceData(SortingType.Note, UseCaseMeta.AddNoteToTimeSlot, loggingMeta, "AnyClass", Guid.NewGuid(),
            "A log",
            DateTimeOffset.Now);
    }

    [Test]
    public void Returns_NoValidationAvailable_When_No_Steps()
    {
        var evaluator = new TraceEvaluator(ImmutableList<VerificationStep>.Empty);
        var result = evaluator.GetResultState(new List<TraceData> { Trace(LoggingMeta.ActionRequested) });
        Assert.That(result, Is.EqualTo(Result.NoValidationAvailable));
    }

    [Test]
    public void Returns_EntityNotFound_If_AggregateNotFound_Present()
    {
        var steps = ImmutableList.Create(new VerificationStep(LoggingMeta.ActionRequested, Invoked.AtLeast, 1));
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData> { Trace(LoggingMeta.AggregateNotFound) };
        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.EntityNotFound));
    }

    [Test]
    public void Returns_Exception_If_ExceptionOccured_Present()
    {
        var steps = ImmutableList.Create(new VerificationStep(LoggingMeta.ActionRequested, Invoked.AtLeast, 1));
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData> { Trace(LoggingMeta.ExceptionOccured) };
        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Exception));
    }

    [Test]
    public void Returns_Aborted_If_ActionAborted_Present()
    {
        var steps = ImmutableList.Create(new VerificationStep(LoggingMeta.ActionRequested, Invoked.AtLeast, 1));
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData> { Trace(LoggingMeta.ActionAborted) };
        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Aborted));
    }

    [Test]
    public void Success_When_All_Equals_Steps_Exactly_Match_Counts()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
            new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.SendingCommand),
            Trace(LoggingMeta.ActionRequested),
            Trace(LoggingMeta.SendingCommand)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Success));
    }

    [Test]
    public void InvalidInvocationCount_When_Equals_Count_Does_Not_Match()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.ActionRequested)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Failed));
    }

    [Test]
    public void Success_When_AtLeast_Requirement_Is_Met()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 1),
            new VerificationStep(LoggingMeta.EventPersisted, Invoked.AtLeast, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.EventPersisted),
            Trace(LoggingMeta.NotificationReceived),
            Trace(LoggingMeta.EventPersisted)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Success));
    }

    [Test]
    public void InvalidInvocationCount_When_AtLeast_Requirement_Not_Met()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.NotificationReceived)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Failed));
    }

    [Test]
    public void Mixed_Equals_And_AtLeast_Are_Handled()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
            new VerificationStep(LoggingMeta.SendingCommand, Invoked.AtLeast, 1),
            new VerificationStep(LoggingMeta.NotificationReceived, Invoked.AtLeast, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.ActionRequested),
            Trace(LoggingMeta.NotificationReceived),
            Trace(LoggingMeta.SendingCommand),
            Trace(LoggingMeta.NotificationReceived)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Success));
    }

    [Test]
    public void Mixed_Equals_Fails_When_Exact_Count_Not_Met()
    {
        var steps = ImmutableList.Create(
            new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1),
            new VerificationStep(LoggingMeta.SendingCommand, Invoked.Equals, 2)
        );
        var evaluator = new TraceEvaluator(steps);

        var traces = new List<TraceData>
        {
            Trace(LoggingMeta.ActionRequested),
            Trace(LoggingMeta.SendingCommand)
        };

        var result = evaluator.GetResultState(traces);

        Assert.That(result, Is.EqualTo(Result.Failed));
    }
}
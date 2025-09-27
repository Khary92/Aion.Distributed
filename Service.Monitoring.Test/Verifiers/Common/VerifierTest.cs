using System.Reflection;
using Moq;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Test.Verifiers.Common;

[TestFixture]
[TestOf(typeof(Verifier))]
public class VerifierTest
{
    [SetUp]
    public void SetUp()
    {
        _traceId = Guid.NewGuid();
        _sortingType = SortingType.Overview;
        _useCaseMeta = UseCaseMeta.CreateNote;
        _traceDataList =
        [
            new TraceData(
                SortingType.Note,
                UseCaseMeta.CreateNote,
                LoggingMeta.ActionRequested,
                "TestClass",
                Guid.NewGuid(),
                "Test log",
                DateTimeOffset.UtcNow)
        ];

        _mockReport = new Report(
            DateTimeOffset.UtcNow,
            _sortingType,
            _useCaseMeta,
            Result.Success,
            150,
            _traceDataList,
            _traceId);

        _reportFactoryMock = new Mock<IReportFactory>();
        _reportFactoryMock
            .Setup(factory => factory.Create(
                _traceId,
                _sortingType,
                _useCaseMeta,
                It.IsAny<List<TraceData>>()))
            .Returns(_mockReport);

        _instance = new Verifier(_traceId, _sortingType, _useCaseMeta, _reportFactoryMock.Object);
    }

    private Mock<IReportFactory> _reportFactoryMock;
    private Guid _traceId;
    private SortingType _sortingType;
    private UseCaseMeta _useCaseMeta;
    private List<TraceData> _traceDataList;
    private Report _mockReport;

    private Verifier _instance;

    [Test]
    public void Add_ShouldStartAndStopTimer()
    {
        var traceData = new TraceData(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.CommandReceived,
            "AnotherClass",
            Guid.NewGuid(),
            "Another log",
            DateTimeOffset.UtcNow);

        Assert.DoesNotThrow(() => _instance.Add(traceData));
    }

    [Test]
    public void VerificationCompleted_ShouldContainCorrectReportValues()
    {
        var eventRaised = false;
        _instance.VerificationCompleted += (sender, report) =>
        {
            eventRaised = true;

            Assert.That(report, Is.Not.Null);
            Assert.That(_traceId, Is.EqualTo(report.TraceId));
            Assert.That(_sortingType, Is.EqualTo(report.SortingType));
            Assert.That(_useCaseMeta, Is.EqualTo(report.UseCase));
            Assert.That(_traceDataList, Is.EqualTo(report.Traces));
        };

        _instance.GetType()
            .GetMethod("Elapsed", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(_instance, [null, null]);

        Assert.That(eventRaised, "VerificationCompleted event was not raised.");
    }

    [Test]
    public void Timer_IsReset_EveryTimeAddIsCalled()
    {
        var traceData = new TraceData(
            SortingType.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.CommandReceived,
            "TestClass",
            Guid.NewGuid(),
            "Test log",
            DateTimeOffset.UtcNow);

        Assert.DoesNotThrow(() =>
        {
            _instance.Add(traceData);
            _instance.Add(traceData);
        });
    }
}
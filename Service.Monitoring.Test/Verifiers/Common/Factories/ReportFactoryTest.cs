using System.Collections.Immutable;
using System.Reflection;
using Moq;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Enums;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Test.Verifiers.Common.Factories
{
    [TestFixture]
    [TestOf(typeof(ReportFactory))]
    public class ReportFactoryTest
    {
        private Mock<IVerificationProvider> _verificationProviderMockOverview;
        private Mock<IVerificationProvider> _verificationProviderMockTicket;
        private ReportFactory _reportFactory;

        [SetUp]
        public void SetUp()
        {
            // Set up mock verification providers
            _verificationProviderMockOverview = new Mock<IVerificationProvider>();
            _verificationProviderMockOverview.Setup(p => p.SortingType).Returns(SortingType.Overview);
            _verificationProviderMockOverview.Setup(p => p.GetVerificationSteps(It.IsAny<UseCaseMeta>()))
                .Returns(ImmutableList.Create(new VerificationStep(LoggingMeta.ActionRequested, Invoked.Equals, 1)));

            _verificationProviderMockTicket = new Mock<IVerificationProvider>();
            _verificationProviderMockTicket.Setup(p => p.SortingType).Returns(SortingType.Ticket);
            _verificationProviderMockTicket.Setup(p => p.GetVerificationSteps(It.IsAny<UseCaseMeta>()))
                .Returns(ImmutableList.Create(new VerificationStep(LoggingMeta.SendingCommand, Invoked.AtLeast, 2)));

            var verificationProviders = new List<IVerificationProvider>
                { _verificationProviderMockOverview.Object, _verificationProviderMockTicket.Object };
            _reportFactory = new ReportFactory(verificationProviders);
        }

        [Test]
        public void Create_ShouldReturnReport_WithCorrectData_ForValidInputs()
        {
            var traceId = Guid.NewGuid();
            var sortingType = SortingType.Overview;
            var useCaseMeta = UseCaseMeta.CreateNote;
            var traceData = new List<TraceData>
            {
                new(sortingType, useCaseMeta, LoggingMeta.ActionRequested, "Origin1", traceId, "LogEntry1",
                    DateTimeOffset.UtcNow.AddMilliseconds(-100)),
                new(sortingType, useCaseMeta, LoggingMeta.ActionRequested, "Origin2", traceId, "LogEntry2",
                    DateTimeOffset.UtcNow)
            };

            var report = _reportFactory.Create(traceId, sortingType, useCaseMeta, traceData);

            Assert.That(report, Is.Not.Null);
            Assert.That(report.TraceId, Is.EqualTo(traceId));
            Assert.That(report.SortingType, Is.EqualTo(sortingType));
            Assert.That(report.UseCase, Is.EqualTo(useCaseMeta));
            Assert.That(report.Traces, Is.EquivalentTo(traceData));
            Assert.That(report.LatencyInMs, Is.EqualTo(100));
        }

        [Test]
        public void Create_ShouldSortTraceData_ByTimeStamp()
        {
            var traceId = Guid.NewGuid();
            var traceData = new List<TraceData>
            {
                new(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, "Origin2",
                    traceId, "LogEntry2", DateTimeOffset.UtcNow),
                new(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, "Origin1",
                    traceId, "LogEntry1", DateTimeOffset.UtcNow.AddMilliseconds(-100))
            };

            var report = _reportFactory.Create(traceId, SortingType.Overview, UseCaseMeta.CreateNote, traceData);

            Assert.That(report.Traces, Is.Ordered.By("TimeStamp"));
        }

        [TestCase(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, Result.Success)]
        [TestCase(SortingType.Ticket, UseCaseMeta.CreateTicket, LoggingMeta.SendingCommand, Result.Failed)]
        public void Create_ShouldDetermineCorrectResultState(SortingType sortingType, UseCaseMeta useCaseMeta,
            LoggingMeta loggingMeta, Result expectedResult)
        {
            var traceId = Guid.NewGuid();
            var traceData = new List<TraceData>
            {
                new TraceData(sortingType, useCaseMeta, loggingMeta, "Origin1", traceId, "LogEntry1",
                    DateTimeOffset.UtcNow)
            };

            var report = _reportFactory.Create(traceId, sortingType, useCaseMeta, traceData);

            Assert.That(report.Result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Create_ShouldThrowKeyNotFoundException_ForInvalidSortingType()
        {
            var traceId = Guid.NewGuid();
            var invalidSortingType = (SortingType)999; 
            var traceData = new List<TraceData>
            {
                new TraceData(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, "Origin1",
                    traceId, "LogEntry1", DateTimeOffset.UtcNow)
            };

            Assert.Throws<KeyNotFoundException>(() =>
                _reportFactory.Create(traceId, invalidSortingType, UseCaseMeta.CreateNote, traceData));
        }

        [Test]
        public void GetLatencyInMs_ShouldReturnCorrectLatency()
        {
            var method = typeof(ReportFactory).GetMethod("GetLatencyInMs",
                BindingFlags.NonPublic | BindingFlags.Static);
            var traceData = new List<TraceData>
            {
                new TraceData(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, "Origin1",
                    Guid.NewGuid(), "LogEntry1", DateTimeOffset.UtcNow.AddMilliseconds(-100)),
                new TraceData(SortingType.Overview, UseCaseMeta.CreateNote, LoggingMeta.ActionRequested, "Origin2",
                    Guid.NewGuid(), "LogEntry2", DateTimeOffset.UtcNow)
            };

            var latency = (int)method!.Invoke(null, [traceData])!;

            Assert.That(latency, Is.EqualTo(100));
        }
        
        [TearDown]
        public void TearDown()
        {
            _verificationProviderMockOverview = null!;
            _verificationProviderMockTicket = null!;
            _reportFactory = null!;
        }
    }
}
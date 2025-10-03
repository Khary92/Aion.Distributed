using Client.Tracing.Tracing.Tracers;
using Client.Tracing.Tracing.Tracers.Client;
using Client.Tracing.Tracing.Tracers.Client.UseCase;
using Client.Tracing.Tracing.Tracers.Note;
using Client.Tracing.Tracing.Tracers.Note.UseCase;
using Client.Tracing.Tracing.Tracers.NoteType;
using Client.Tracing.Tracing.Tracers.NoteType.UseCase;
using Client.Tracing.Tracing.Tracers.Sprint;
using Client.Tracing.Tracing.Tracers.Sprint.UseCase;
using Client.Tracing.Tracing.Tracers.Statistics;
using Client.Tracing.Tracing.Tracers.Statistics.UseCase;
using Client.Tracing.Tracing.Tracers.Tag;
using Client.Tracing.Tracing.Tracers.Tag.UseCase;
using Client.Tracing.Tracing.Tracers.Ticket;
using Client.Tracing.Tracing.Tracers.Ticket.UseCase;
using Client.Tracing.Tracing.Tracers.TimeSlot;
using Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;
using Client.Tracing.Tracing.Tracers.WorkDay;
using Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

namespace Client.Tracing.Mock;

public class DummyTracer : ITraceCollector
{
    public ITicketUseCaseSelector Ticket { get; } = new MockTicketUseCaseSelector();
    public INoteTypeUseCaseSelector NoteType { get; } = new MockNoteTypeUseCaseSelector();
    public ISprintUseCaseSelector Sprint { get; } = new MockSprintUseCaseSelector();
    public ITagUseCaseSelector Tag { get; } = new MockTagUseCaseSelector();
    public INoteUseCaseSelector Note { get; } = new MockNoteUseCaseSelector();
    public IWorkDayUseCaseSelector WorkDay { get; } = new MockWorkDayUseCaseSelector();
    public IStatisticsDataUseCaseSelector Statistics { get; } = new MockStatisticsUseCaseSelector();
    public ITimeSlotUseCaseSelector TimeSlot { get; } = new MockTimeSlotUseCaseSelector();
    public IClientUseCaseSelector Client { get; } = new MockClientUseCaseSelector();

    private class MockTicketUseCaseSelector : ITicketUseCaseSelector
    {
        public IUpdateTicketDocuTraceCollector ChangeDocumentation { get; } = new DummyTraceCollector();
        public ICreateTicketUseCaseCollector Create { get; } = new DummyTraceCollector();
        public IUpdateTicketTraceCollector Update { get; } = new DummyTraceCollector();
    }

    private class MockNoteTypeUseCaseSelector : INoteTypeUseCaseSelector
    {
        public ICreateNoteTypeTraceCollector Create { get; } = new DummyTraceCollector();
        public IChangeNoteTypeColorTraceCollector ChangeColor { get; } = new DummyTraceCollector();
        public IChangeNoteTypeNameTraceCollector ChangeName { get; } = new DummyTraceCollector();
    }

    private class MockSprintUseCaseSelector : ISprintUseCaseSelector
    {
        public ICreateSprintTraceCollector Create { get; } = new DummyTraceCollector();
        public IUpdateSprintCollector Update { get; } = new DummyTraceCollector();
        public ISprintActiveStatusCollector ActiveStatus { get; } = new DummyTraceCollector();
        public ITicketAddedToSprintCollector AddTicketToSprint { get; } = new DummyTraceCollector();
    }

    private class MockTagUseCaseSelector : ITagUseCaseSelector
    {
        public ICreateTagTraceCollector Create { get; } = new DummyTraceCollector();
        public IUpdateTagTraceCollector Update { get; } = new DummyTraceCollector();
    }

    private class MockNoteUseCaseSelector : INoteUseCaseSelector
    {
        public ICreateNoteTraceCollector Create { get; } = new DummyTraceCollector();
        public IUpdateNoteTraceCollector Update { get; } = new DummyTraceCollector();
    }

    private class MockWorkDayUseCaseSelector : IWorkDayUseCaseSelector
    {
        public ICreateWorkDayTraceCollector Create { get; } = new DummyTraceCollector();
    }

    private class MockStatisticsUseCaseSelector : IStatisticsDataUseCaseSelector
    {
        public IChangeProductivityTraceCollector ChangeProductivity { get; } = new DummyTraceCollector();
        public IChangeTagSelectionTraceCollector ChangeTagSelection { get; } = new DummyTraceCollector();
    }

    private class MockTimeSlotUseCaseSelector : ITimeSlotUseCaseSelector
    {
        public ISetEndTimeTraceCollector SetEndTime { get; } = new DummyTraceCollector();
        public ISetStartTimeTraceCollector SetStartTime { get; } = new DummyTraceCollector();
    }

    private class MockClientUseCaseSelector : IClientUseCaseSelector
    {
        public ICreateTrackingControlCollector CreateTrackingControl { get; } = new DummyTraceCollector();
    }

    private class DummyTraceCollector :
        IUpdateTicketDocuTraceCollector,
        ICreateTicketUseCaseCollector,
        IUpdateTicketTraceCollector,
        ICreateNoteTypeTraceCollector,
        IChangeNoteTypeColorTraceCollector,
        IChangeNoteTypeNameTraceCollector,
        ICreateSprintTraceCollector,
        IUpdateSprintCollector,
        ISprintActiveStatusCollector,
        ITicketAddedToSprintCollector,
        ICreateTagTraceCollector,
        IUpdateTagTraceCollector,
        ICreateNoteTraceCollector,
        IUpdateNoteTraceCollector,
        ICreateWorkDayTraceCollector,
        IChangeProductivityTraceCollector,
        IChangeTagSelectionTraceCollector,
        ISetEndTimeTraceCollector,
        ISetStartTimeTraceCollector,
        ICreateTrackingControlCollector
    {
        Task IChangeNoteTypeColorTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeNoteTypeColorTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeNoteTypeColorTraceCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeNoteTypeNameTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeNoteTypeNameTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeNoteTypeNameTraceCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeProductivityTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeProductivityTraceCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeProductivityTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeProductivityTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeProductivityTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeTagSelectionTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeTagSelectionTraceCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeTagSelectionTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeTagSelectionTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        public Task WrongModel(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IChangeTagSelectionTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTraceCollector.ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTypeTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateNoteTypeTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateSprintTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateSprintTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTagTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTagTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTicketUseCaseCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTicketUseCaseCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTrackingControlCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTrackingControlCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTrackingControlCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTrackingControlCollector.ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateTrackingControlCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateWorkDayTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateWorkDayTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        public Task ActionAborted(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateWorkDayTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ICreateWorkDayTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetEndTimeTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetEndTimeTraceCollector.CacheIsEmpty(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        public Task FlushingCacheFailed(Type originClassType, Guid traceId, string filePath)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetEndTimeTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetStartTimeTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetStartTimeTraceCollector.CacheIsEmpty(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISetStartTimeTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISprintActiveStatusCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISprintActiveStatusCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ISprintActiveStatusCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ITicketAddedToSprintCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ITicketAddedToSprintCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task ITicketAddedToSprintCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateNoteTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateSprintCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateSprintCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateSprintCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTagTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTagTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTagTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketDocuTraceCollector.StartUseCase(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketDocuTraceCollector.SendingCommand(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketDocuTraceCollector.AggregateAdded(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketDocuTraceCollector.NotificationReceived(Type originClassType, Guid traceId,
            object notification)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        public Task ActionAborted(Type originClassType, Guid traceId, string reason)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketTraceCollector.NoAggregateFound(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketTraceCollector.ChangesApplied(Type originClassType, Guid traceId)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }

        Task IUpdateTicketTraceCollector.NotificationReceived(Type originClassType, Guid traceId, object command)
        {
            //Maybe add some logging if needed
            return Task.CompletedTask;
        }
    }
}
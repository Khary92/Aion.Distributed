using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Settings;
using Client.Tracing.Tracing.Tracers;
using Moq;
using IMessenger = CommunityToolkit.Mvvm.Messaging.IMessenger;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace Client.Desktop.Test.Presentation.Models.Settings;

public static class WorkDaysModelProvider
{
    private static IMessenger CreateMessenger()
    {
        return new WeakReferenceMessenger();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    private static Mock<ICommandSender> CreateCommandSender()
    {
        return new Mock<ICommandSender>();
    }

    private static Mock<ILocalSettingsCommandSender> CreateLocalSettingsCommandSenderMock()
    {
        return new Mock<ILocalSettingsCommandSender>();
    }

    public static async Task<WorkDaysModelFixture> Create(List<WorkDayClientModel> initialWorkDays,
        bool isWorkDayExisting)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var localSettingsCommandSender = CreateCommandSender();
        var localSettingsCommandSenderMock = CreateLocalSettingsCommandSenderMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllWorkDaysRequest>()))
            .ReturnsAsync(initialWorkDays);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientIsWorkDayExistingRequest>()))
            .ReturnsAsync(isWorkDayExisting);

        return await CreateFixture(messenger, localSettingsCommandSender, requestSender, tracer,
            localSettingsCommandSenderMock);
    }

    private static async Task<WorkDaysModelFixture> CreateFixture(IMessenger messenger,
        Mock<ICommandSender> commandSender,
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer, Mock<ILocalSettingsCommandSender> localSettingsCommandSender)
    {
        var instance = new WorkDaysModel(commandSender.Object, requestSender.Object,
            localSettingsCommandSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new WorkDaysModelFixture
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger,
            LocalSettingsCommandSender = localSettingsCommandSender,
            CommandSender = commandSender
        };
    }

    public sealed class WorkDaysModelFixture
    {
        public required WorkDaysModel Instance { get; init; }
        public required IMessenger Messenger { get; init; }

        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ILocalSettingsCommandSender> LocalSettingsCommandSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
    }
}
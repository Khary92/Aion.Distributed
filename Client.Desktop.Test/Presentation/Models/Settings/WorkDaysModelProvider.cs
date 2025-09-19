using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using Moq;
using IMessenger = CommunityToolkit.Mvvm.Messaging.IMessenger;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace Client.Desktop.Test.Presentation.Models.Settings;

public static class WorkDaysModelProvider
{
    public sealed class WorkDaysModelFixture
    {
        public required WorkDaysModel Instance { get; init; }
        public required IMessenger Messenger { get; init; }

        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ILocalSettingsCommandSender> LocalSettingsCommandSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
    }

    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    private static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    private static Mock<IRequestSender> CreateRequestSenderMock() => new();
    private static Mock<ICommandSender> CreateCommandSender() => new();

    private static Mock<ILocalSettingsCommandSender> CreateLocalSettingsCommandSenderMock() => new();

    public static async Task<WorkDaysModelFixture> Create()
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var localSettingsCommandSender = CreateCommandSender();
        var localSettingsCommandSenderMock = CreateLocalSettingsCommandSenderMock();

        return await CreateFixture(messenger, localSettingsCommandSender, requestSender, tracer,
            localSettingsCommandSenderMock);
    }

    private static async Task<WorkDaysModelFixture> CreateFixture(IMessenger messenger,
        Mock<ICommandSender> commandSender,
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer, Mock<ILocalSettingsCommandSender> localSettingsCommandSender)
    {
        var instance = new WorkDaysModel(messenger, commandSender.Object, requestSender.Object,
            localSettingsCommandSender.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new WorkDaysModelFixture()
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger,
            LocalSettingsCommandSender = localSettingsCommandSender,
            CommandSender = commandSender,
        };
    }
}
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Local;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Settings;

public static class SettingsModelProvider
{
    public sealed class SettingsModelFixture
    {
        public required SettingsModel Instance { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<ILocalSettingsCommandSender> LocalSettingsCommandSender { get; init; }
        
        public required SettingsClientModel InitialSettings { get; init; }
    }

    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();
    private static Mock<ILocalSettingsCommandSender> CreateLocalSettingsCommandSenderMock() => new();

    private static SettingsClientModel CreateSettingsClientModel() => new("InitialExportPath");

    public static SettingsModelFixture Create()
    {
        var messenger = CreateMessenger();
        var localSettingsCommandSender = CreateLocalSettingsCommandSenderMock();

        return CreateFixture(messenger, localSettingsCommandSender);
    }

    private static SettingsModelFixture CreateFixture(IMessenger messenger,
        Mock<ILocalSettingsCommandSender> localSettingsCommandSender)
    {
        var instance = new SettingsModel(messenger, localSettingsCommandSender.Object);

        instance.RegisterMessenger();

        var settingsClientModel = CreateSettingsClientModel();
        
        messenger.Send(settingsClientModel);

        return new SettingsModelFixture()
        {
            Instance = instance,
            LocalSettingsCommandSender = localSettingsCommandSender,
            Messenger = messenger,
            InitialSettings = settingsClientModel,
        };
    }
}
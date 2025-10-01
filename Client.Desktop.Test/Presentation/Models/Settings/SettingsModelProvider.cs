using Client.Desktop.DataModels.Local;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Settings;

public static class SettingsModelProvider
{
    private static Mock<ILocalSettingsService> CreateLocalSettingsService()
    {
        return new Mock<ILocalSettingsService>();
    }


    private static SettingsClientModel CreateSettingsClientModel()
    {
        return new SettingsClientModel("InitialExportPath");
    }

    public static SettingsModelFixture Create()
    {
        var localSettingsService = CreateLocalSettingsService();

        return CreateFixture(localSettingsService);
    }

    private static SettingsModelFixture CreateFixture(Mock<ILocalSettingsService> localSettingsService)
    {
        var instance = new SettingsModel(localSettingsService.Object);

        var settingsClientModel = CreateSettingsClientModel();

        return new SettingsModelFixture
        {
            Instance = instance,
            InitialSettings = settingsClientModel,
            LocalSettingsService = localSettingsService
        };
    }

    public sealed class SettingsModelFixture
    {
        public required SettingsModel Instance { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
        public required SettingsClientModel InitialSettings { get; init; }
    }
}
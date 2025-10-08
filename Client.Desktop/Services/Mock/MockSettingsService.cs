using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

namespace Client.Desktop.Services.Mock;

public class MockSettingsService(
    IFileSystemWriter writer,
    IFileSystemReader reader,
    IFileSystemWrapper fileSystemWrapper)
    : IMockSettingsService, IInitializeAsync
{
    private const string SettingsFileName = "MockSettings.json";

    public InitializationType Type => InitializationType.MockServices;

    public async Task InitializeAsync()
    {
        if (fileSystemWrapper.IsFileExisting(SettingsFileName))
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
                Settings = await reader.GetObject<MockSettings>(SettingsFileName));
            return;
        }

        await SaveSettings(Settings);
    }

    public MockSettings Settings { get; private set; } = new(false);
    public bool IsMockingModeActive { get; set; }

    public async Task SaveSettings(MockSettings changedSettings)
    {
        await writer.Write(JsonSerializer.Serialize(changedSettings), SettingsFileName);
    }
}
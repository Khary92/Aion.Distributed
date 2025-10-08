using System.Threading.Tasks;

namespace Client.Desktop.Services.Mock;

public interface IMockSettingsService
{
    MockSettings Settings { get; }
    Task SaveSettings(MockSettings changedSettings);
    bool IsMockingModeActive { get; set; }
}
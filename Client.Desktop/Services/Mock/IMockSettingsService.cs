using System.Threading.Tasks;

namespace Client.Desktop.Services.Mock;

public interface IMockSettingsService
{
    MockSettings Settings { get; }
    bool IsMockingModeActive { get; set; }
    Task SaveSettings(MockSettings changedSettings);
}
using System.Threading.Tasks;
using Client.Desktop.Communication.Mock.DataProvider;

namespace Client.Desktop.Services.Mock;

public interface IMockSeedSetupService
{
    bool IsClearSetup { get; set; }
    Task<MockSetup> ReadSetupFromFile();
    Task SaveSettings(MockSetup changedSettings);
}
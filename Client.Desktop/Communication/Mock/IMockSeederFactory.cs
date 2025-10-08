using Client.Desktop.Communication.Mock.DataProvider;

namespace Client.Desktop.Communication.Mock;

public interface IMockSeederFactory
{
    IMockSeeder Create(MockSetup setup);
}
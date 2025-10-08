using Client.Desktop.Communication.Mock.DataProvider;

namespace Client.Desktop.Communication.Mock;

public class MockSeederFactory : IMockSeederFactory
{
    public IMockSeeder Create(MockSetup setup)
    {
        var mockSeeder = new MockSeeder();
        //mockSeeder.Seed(setup);
        return mockSeeder;
    }
}
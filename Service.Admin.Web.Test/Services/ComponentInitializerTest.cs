using Moq;
using Service.Admin.Web.Services;
using Service.Admin.Web.Services.Startup;

namespace Service.Admin.Web.Test.Services;

[TestFixture]
[TestOf(typeof(ComponentInitializer))]
public class ComponentInitializerTest
{
    [Test]
    public async Task InitializeServicesAsync_ShouldInitializeComponentsInOrder()
    {
        var stateServiceMock = new Mock<IInitializeAsync>();
        stateServiceMock.SetupGet(c => c.Type).Returns(InitializationType.StateService);

        var controllerMock = new Mock<IInitializeAsync>();
        controllerMock.SetupGet(c => c.Type).Returns(InitializationType.Controller);

        var initializer = new ComponentInitializer(new[] { stateServiceMock.Object, controllerMock.Object });

        await initializer.InitializeServicesAsync();

        stateServiceMock.Verify(c => c.InitializeComponents(), Times.Once);
        controllerMock.Verify(c => c.InitializeComponents(), Times.Once);
    }
}
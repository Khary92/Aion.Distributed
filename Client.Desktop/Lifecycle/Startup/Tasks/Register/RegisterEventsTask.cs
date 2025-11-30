using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Register;

public class RegisterEventsTask(IEnumerable<IEventRegistration> messengerComponents) : IStartupTask
{
    public StartupTask StartupTask => StartupTask.RegisterMessenger;

    public Task Execute()
    {
        foreach (var component in messengerComponents)
            try
            {
                component.RegisterMessenger();
            }
            catch (Exception ex)
            {
                throw new InitializationException($"Failed to register messenger for {component.GetType().Name}", ex);
            }

        return Task.CompletedTask;
    }
}
namespace Client.Desktop.Lifecycle.Startup.Tasks.Register;

public interface IEventRegistration
{
    void RegisterMessenger();
    public void UnregisterMessenger();
}
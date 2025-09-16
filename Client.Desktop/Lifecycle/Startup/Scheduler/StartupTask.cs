namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public enum StartupTask
{
    RegisterMessenger,
    NotificationStream,
    CheckUnsentCommands,
    AsyncInitialize
}
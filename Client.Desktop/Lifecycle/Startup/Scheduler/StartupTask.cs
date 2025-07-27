namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public enum StartupTask
{
    RegisterMessenger,
    CheckUnsentCommands,
    AsyncInitialize,
    RegisterStreams
}
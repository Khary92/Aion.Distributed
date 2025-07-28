using System;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

public class InitializationException(string message, Exception? innerException = null)
    : Exception(message, innerException);
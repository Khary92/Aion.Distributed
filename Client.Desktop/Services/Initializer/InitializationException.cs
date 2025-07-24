using System;

namespace Client.Desktop.Services.Initializer;

public class InitializationException(string message, Exception? innerException = null)
    : Exception(message, innerException);
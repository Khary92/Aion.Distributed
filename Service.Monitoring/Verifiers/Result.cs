namespace Service.Monitoring.Verifiers;

public enum Result
{
    Success,
    Exception,
    Aborted,
    NotFound,
    TimedOut,
    WrongOrder,
    InvalidInvocationCount
}
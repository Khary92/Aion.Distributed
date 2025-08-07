namespace Service.Monitoring.Verifiers.Common.Enums;

public enum Result
{
    Success,
    Exception,
    Aborted,
    NotFound,
    TimedOut,
    WrongOrder,
    InvalidInvocationCount,
    NoValidationAvailable
}
namespace Service.Monitoring.Verifiers.Common.Enums;

public enum Result
{
    Success,
    Exception,
    NotFound,
    TimedOut,
    WrongOrder,
    InvalidInvocationCount,
    NoValidationAvailable
}
namespace Service.Monitoring.Verifiers.Common.Enums;

public enum Result
{
    Success,
    Exception,
    EntityNotFound,
    TimedOut,
    InvalidInvocationCount,
    NoValidationAvailable,
    Aborted,
    OngoingValidation
}
namespace Service.Monitoring.Verifiers.Common.Enums;

public enum Result
{
    Success,
    Exception,
    EntityNotFound,
    InvalidInvocationCount,
    NoValidationAvailable,
    Aborted,
    OngoingValidation
}
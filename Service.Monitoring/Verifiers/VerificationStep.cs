using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Verifiers;

public record VerificationStep(LoggingMeta LoggingMeta, Invoked Invoked, int Count);
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;

namespace Service.Monitoring.Verifiers.Common.Records;

public record VerificationStep(LoggingMeta LoggingMeta, Invoked Invoked, int Count);
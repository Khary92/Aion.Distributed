using System;

namespace Client.Desktop.Communication.Requests.Client.Records;

public record ClientGetTrackingControlDataRequest(DateTimeOffset Date, Guid TraceId);
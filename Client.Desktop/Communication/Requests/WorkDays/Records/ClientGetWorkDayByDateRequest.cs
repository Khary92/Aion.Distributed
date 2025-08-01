using System;

namespace Client.Desktop.Communication.Requests.WorkDays.Records;

public record ClientGetWorkDayByDateRequest(DateTimeOffset Date, Guid TraceId);
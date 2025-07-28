using System;

namespace Client.Desktop.Communication.Requests.WorkDays.Records;

public record ClientIsWorkDayExistingRequest(DateTimeOffset Date);
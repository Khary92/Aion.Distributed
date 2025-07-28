using System;

namespace Client.Desktop.Communication.Requests.UseCase.Records;

public record ClientGetTimeSlotControlDataRequest(DateTimeOffset Date);
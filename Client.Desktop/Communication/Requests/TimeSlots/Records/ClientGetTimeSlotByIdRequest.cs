using System;

namespace Client.Desktop.Communication.Requests.TimeSlots.Records;

public record ClientGetTimeSlotByIdRequest(Guid TimeSlotId);
using System;

namespace Client.Desktop.Communication.Requests.TimeSlots.Records;

public record ClientGetTimeSlotsForWorkDayIdRequest(Guid WorkDayId);
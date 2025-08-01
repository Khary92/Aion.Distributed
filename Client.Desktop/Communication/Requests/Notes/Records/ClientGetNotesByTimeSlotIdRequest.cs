using System;

namespace Client.Desktop.Communication.Requests.Notes.Records;

public record ClientGetNotesByTimeSlotIdRequest(Guid TimeSlotId, Guid TraceId);
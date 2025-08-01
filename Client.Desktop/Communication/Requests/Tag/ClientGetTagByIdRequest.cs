using System;

namespace Client.Desktop.Communication.Requests.Tag;

public record ClientGetTagByIdRequest(Guid TagId, Guid TraceId);
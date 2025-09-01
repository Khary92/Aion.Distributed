using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.Requests.Tag;

public record ClientGetTagsByIdsRequest(List<Guid> TagIds);
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Wrappers;

public record NewTagMessage(TagWebModel Tag, Guid TraceId);
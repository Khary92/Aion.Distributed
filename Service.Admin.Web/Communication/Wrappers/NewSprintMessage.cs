using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Wrappers;

public record NewSprintMessage(SprintWebModel Sprint, Guid TraceId);
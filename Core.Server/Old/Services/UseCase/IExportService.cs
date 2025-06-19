using System.Collections.ObjectModel;

namespace Service.Server.Old.Services.UseCase;

public interface IExportService
{
    Task<bool> ExportToFile(Collection<WorkDayDto> workDayDtos);
    Task<string> GetMarkdownString(Collection<WorkDayDto> workDayDtos);
}
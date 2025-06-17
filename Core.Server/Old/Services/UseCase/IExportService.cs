using System.Collections.ObjectModel;
using Application.Contract.DTO;

namespace Application.Services.UseCase;

public interface IExportService
{
    Task<bool> ExportToFile(Collection<WorkDayDto> workDayDtos);
    Task<string> GetMarkdownString(Collection<WorkDayDto> workDayDtos);
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.DTO;

namespace Client.Desktop.Services;

public interface IExportService
{
    Task<bool> ExportToFile(Collection<WorkDayDto> workDayDtos);
    Task<string> GetMarkdownString(Collection<WorkDayDto> workDayDtos);
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.RequiresChange;

public interface IExportService
{
    Task<bool> ExportToFile(Collection<WorkDayDto> workDayDtos);
    Task<string> GetMarkdownString(Collection<WorkDayDto> workDayDtos);
}
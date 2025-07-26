using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.DataModels;

namespace Client.Desktop.Services;

public interface IExportService
{
    Task<bool> ExportToFile(Collection<WorkDayClientModel> workDayDtos);
    Task<string> GetMarkdownString(Collection<WorkDayClientModel> workDayDtos);
}
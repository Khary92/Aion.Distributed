using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisControlWrapperViewModel(
    AnalysisByTagViewModel analysisByTagViewModel,
    AnalysisByTicketViewModel analysisByTicketViewModel,
    AnalysisBySprintViewModel analysisBySprintViewModel) : ReactiveObject
{
    public AnalysisByTagViewModel AnalysisByTagViewModel => analysisByTagViewModel;
    public AnalysisByTicketViewModel AnalysisByTicketViewModel => analysisByTicketViewModel;
    public AnalysisBySprintViewModel AnalysisBySprintViewModel => analysisBySprintViewModel;
}
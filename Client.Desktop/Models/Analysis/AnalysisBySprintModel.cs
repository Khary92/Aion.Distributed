using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Desktop.Communication.RequiresChange;
using Contract.Decorators;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

public class AnalysisBySprintModel : ReactiveObject
{
    private const int AmountOfTagsShown = 3;

    private readonly IAnalysisDataService _analysisDataService;
    private readonly IMediator _mediator;

    private AnalysisBySprintDecorator? _analysisBySprint;

    public AnalysisBySprintModel(IMediator mediator, IAnalysisDataService analysisDataService)
    {
        _mediator = mediator;
        _analysisDataService = analysisDataService;

        InitializeAsync().ConfigureAwait(false);
    }

    public ObservableCollection<SprintDto> Sprints { get; } = [];

    public AnalysisBySprintDecorator? AnalysisBySprint
    {
        get => _analysisBySprint;
        private set => this.RaiseAndSetIfChanged(ref _analysisBySprint, value);
    }

    public string GetMarkdownString()
    {
        if (AnalysisBySprint == null) return string.Empty;

        var builder = new StringBuilder();

        builder.AppendLine($"### Overview for {AnalysisBySprint.SprintName}");
        builder.AppendLine();
        builder.AppendLine(GetProductiveTable());
        builder.AppendLine();
        builder.AppendLine(GetNeutralTable());
        builder.AppendLine();
        builder.AppendLine(GetUnproductiveTable());

        return builder.ToString();
    }

    private string GetProductiveTable()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"#### Top {AmountOfTagsShown} most associated tags for Productive");
        builder.AppendLine();
        builder.AppendLine("| Tag Name | Count |");
        builder.AppendLine("|----------|--------|");

        if (AnalysisBySprint!.ProductiveTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }
        
        var count = 0;
        foreach (var pair in AnalysisBySprint!.ProductiveTags.OrderByDescending(kvp => kvp.Value))
        {
            if (count == AmountOfTagsShown - 1) break;

            builder.AppendLine($"| {pair.Key} | {pair.Value} |");
            count++;
        }

        return builder.ToString();
    }

    private string GetNeutralTable()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"#### Top {AmountOfTagsShown} most associated tags for Neutral");
        builder.AppendLine();
        builder.AppendLine("| Tag Name | Count |");
        builder.AppendLine("|----------|--------|");

        if (AnalysisBySprint!.NeutralTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }

        var count = 0;
        foreach (var pair in AnalysisBySprint!.NeutralTags.OrderByDescending(kvp => kvp.Value))
        {
            if (count == AmountOfTagsShown - 1) break;

            builder.AppendLine($"| {pair.Key} | {pair.Value} |");
            count++;
        }

        return builder.ToString();
    }

    private string GetUnproductiveTable()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"#### Top {AmountOfTagsShown} most associated tags for Unproductive");
        builder.AppendLine();
        builder.AppendLine("| Tag Name | Count |");
        builder.AppendLine("|----------|--------|");

        if (AnalysisBySprint!.UnproductiveTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }

        var count = 0;
        foreach (var pair in AnalysisBySprint!.UnproductiveTags.OrderByDescending(kvp => kvp.Value))
        {
            if (count == AmountOfTagsShown - 1) break;

            builder.AppendLine($"| {pair.Key} | {pair.Value} |");
            count++;
        }

        return builder.ToString();
    }

    private async Task InitializeAsync()
    {
        Sprints.Clear();
       // Sprints.AddRange(await _mediator.Send(new GetAllSprintsRequest()));
    }

    public async Task SetAnalysisForSprint(SprintDto selectedSprint)
    {
        AnalysisBySprint = await _analysisDataService.GetAnalysisBySprint(selectedSprint);
    }
}
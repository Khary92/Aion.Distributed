using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaEdit.Utils;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Decorators;
using Client.Desktop.DTO;
using Client.Desktop.Services.Initializer;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.Sprints;
using Proto.Requests.AnalysisData;
using Proto.Requests.Sprints;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

public class AnalysisBySprintModel(IMessenger messenger, IRequestSender requestSender)
    : ReactiveObject, IRegisterMessenger, IInitializeAsync
{
    private const int AmountOfTagsShown = 3;

    private AnalysisBySprintDecorator? _analysisBySprint;

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

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints!.AddRange(await requestSender.Send(new GetAllSprintsRequestProto()));
    }

    public async Task SetAnalysisForSprint(SprintDto selectedSprint)
    {
        AnalysisBySprint = await requestSender.Send(new GetSprintAnalysisById
        {
            SprintId = selectedSprint.SprintId.ToString()
        });
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewSprintMessage>(this, (_, m) => { Sprints.Add(m.Sprint); });

        messenger.Register<UpdateSprintDataCommandProto>(this, (_, m) =>
        {
            var sprint = Sprints.FirstOrDefault(s => s.SprintId == Guid.Parse(m.SprintId));

            if (sprint == null) return;

            sprint.Apply(m);
        });
    }
}
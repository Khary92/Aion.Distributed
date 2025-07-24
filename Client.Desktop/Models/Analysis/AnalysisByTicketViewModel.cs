using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Proto;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Client.Desktop.Models.Analysis;

public class AnalysisByTicketViewModel(AnalysisByTicketModel analysisByTicketModel) : ReactiveObject
{
    private string _mostRelevantTagsByProductivityText = string.Empty;

    private ObservableCollection<ISeries> _productivityTimeSpentBarChart = [];

    private TicketDto? _selectedTicket;

    public AnalysisByTicketModel Model { get; } = analysisByTicketModel;

    public TicketDto? SelectedTicket
    {
        get => _selectedTicket;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTicket, value);
            _ = LoadAnalysisByTicketAsync();
        }
    }

    public ObservableCollection<PieSeries<double>> ProductivityByTimeSpentPieChart { get; } = [];

    public ObservableCollection<ISeries> ProductivityTimeSpentBarChart
    {
        get => _productivityTimeSpentBarChart;
        set => this.RaiseAndSetIfChanged(ref _productivityTimeSpentBarChart, value);
    }

    public Axis[] ProductivityTimeSpentBarChartXAxisNames { get; set; } =
    [
        new()
        {
            Labels = ["Productive", "Neutral", "Unproductive"],
            Labeler = value => value.ToString(CultureInfo.InvariantCulture),
            IsVisible = true,
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    ];

    public Axis[] ProductivityTimeSpentBarChartYAxis { get; } =
    [
        new()
        {
            Labeler = value => value.ToString(CultureInfo.InvariantCulture),
            IsVisible = true,
            MinLimit = 0
        }
    ];

    public string MostRelevantTagsByProductivityText
    {
        get => _mostRelevantTagsByProductivityText;
        set => this.RaiseAndSetIfChanged(ref _mostRelevantTagsByProductivityText, value);
    }

    private async Task LoadAnalysisByTicketAsync()
    {
        if (SelectedTicket is null) return;

        await Model.SetAnalysisByTicket(SelectedTicket);

        UpdateProductivityByTimeSpentPieChart();
        UpdateProductivityByTimeSpentBarChart();
        await UpdateMostRelevantTagsByProductivityText();
    }

    private void UpdateProductivityByTimeSpentPieChart()
    {
        if (Model.AnalysisByTicket == null) return;

        var percentageData = Model.AnalysisByTicket.GetTimeSpentInProductivity();

        ProductivityByTimeSpentPieChart.Clear();

        ProductivityByTimeSpentPieChart.Add(new PieSeries<double>
        {
            Name = "Productive",
            Values = new List<double> { percentageData[Productivity.Productive] },
            Fill = new SolidColorPaint(SKColors.Green),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 14,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Productive"
        });

        ProductivityByTimeSpentPieChart.Add(new PieSeries<double>
        {
            Name = "Neutral",
            Values = new List<double> { percentageData[Productivity.Neutral] },
            Fill = new SolidColorPaint(SKColors.Yellow),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 14,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Neutral"
        });

        ProductivityByTimeSpentPieChart.Add(new PieSeries<double>
        {
            Name = "Unproductive",
            Values = new List<double> { percentageData[Productivity.Unproductive] },
            Fill = new SolidColorPaint(SKColors.Red),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 14,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Unproductive"
        });
    }

    private void UpdateProductivityByTimeSpentBarChart()
    {
        if (Model.AnalysisByTicket == null) return;

        var percentageData = Model.AnalysisByTicket.GetTimeSpentInProductivity();


        ProductivityTimeSpentBarChart.Clear();

        ProductivityTimeSpentBarChart.Add(new ColumnSeries<int>
        {
            Name = "Productive",
            Values = new List<int> { percentageData[Productivity.Productive], 0, 0 },
            Fill = new SolidColorPaint(SKColors.Green),
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 14
        });

        ProductivityTimeSpentBarChart.Add(new ColumnSeries<int>
        {
            Name = "Neutral",
            Values = new List<int> { 0, percentageData[Productivity.Neutral], 0 },
            Fill = new SolidColorPaint(SKColors.Yellow),
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 14
        });

        ProductivityTimeSpentBarChart.Add(new ColumnSeries<int>
        {
            Name = "Unproductive",
            Values = new List<int> { 0, 0, percentageData[Productivity.Unproductive] },
            Fill = new SolidColorPaint(SKColors.Red),
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 14
        });
    }

    private async Task UpdateMostRelevantTagsByProductivityText()
    {
        if (Model.AnalysisByTicket == null) return;

        var tagData = await Model.AnalysisByTicket.GetMostRepresentedTagIdsByProductivity();
        var builder = new StringBuilder();

        builder.AppendLine(
            $"### Total time spent with ticket -{Model.AnalysisByTicket.TicketName}- is {TimeSpan.FromMinutes(Model.AnalysisByTicket.TotalTimeSpent):hh\\:mm}h");
        builder.AppendLine();


        foreach (var keyValue in tagData)
        {
            builder.AppendLine($"#### Top 3 most associated tags for {keyValue.Key}");
            builder.AppendLine();
            builder.AppendLine("| Tag Name | Count |");
            builder.AppendLine("|----------|--------|");

            if (keyValue.Value.Count == 0)
            {
                builder.AppendLine("| None | Not available |");
                builder.AppendLine();
                continue;
            }

            foreach (var tagId in keyValue.Value)
            {
                var tag = await Model.GetTagById(tagId);
                var count = Model.AnalysisByTicket.CountTagByProductivity(keyValue.Key, tagId);
                builder.AppendLine($"| {tag.Name} | {count} |");
            }

            builder.AppendLine();
        }

        MostRelevantTagsByProductivityText = builder.ToString();
    }
}
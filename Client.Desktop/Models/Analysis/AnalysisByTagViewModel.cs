using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Client.Desktop.DTO;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Proto.Client;
using ReactiveUI;
using SkiaSharp;

namespace Client.Desktop.Models.Analysis;

public class AnalysisByTagViewModel : ReactiveObject
{
    private ObservableCollection<ISeries> _productivityTimeSpentBarChart = [];

    private TagDto? _selectedTag;

    public AnalysisByTagViewModel()
    {
    }

    public AnalysisByTagViewModel(AnalysisByTagModel analysisByTagModel)
    {
        Model = analysisByTagModel;
    }

    public AnalysisByTagModel Model { get; } = null!;

    public TagDto? SelectedTag
    {
        get => _selectedTag;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTag, value);
            LoadAnalysisByTagAsync();
        }
    }

    public ObservableCollection<PieSeries<double>> ProductivityPieChart { get; } = [];

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

    private async void LoadAnalysisByTagAsync()
    {
        if (SelectedTag is null) return;

        await Model.SetAnalysisForTag(SelectedTag);
        UpdateProductivityPieChart();
        UpdateProductivityByTimeSpentPieChart();
        UpdateProductivityByTimeSpentBarChart();
    }


    private void UpdateProductivityPieChart()
    {
        if (Model.AnalysisByTag == null) return;

        var percentageData = Model.AnalysisByTag.GetProductivityPercentages();

        ProductivityPieChart.Clear();

        ProductivityPieChart.Add(new PieSeries<double>
        {
            Name = "Productive",
            Values = new List<double> { percentageData[Productivity.Productive] },
            Fill = new SolidColorPaint(SKColors.Green),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 14,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Productive"
        });

        ProductivityPieChart.Add(new PieSeries<double>
        {
            Name = "Neutral",
            Values = new List<double> { percentageData[Productivity.Neutral] },
            Fill = new SolidColorPaint(SKColors.Yellow),
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 14,
            DataLabelsPosition = PolarLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Neutral"
        });

        ProductivityPieChart.Add(new PieSeries<double>
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

    private void UpdateProductivityByTimeSpentPieChart()
    {
        if (Model.AnalysisByTag == null) return;

        var percentageData = Model.AnalysisByTag.GetProductivityPercentageByTime();

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
        if (Model.AnalysisByTag == null) return;

        var percentageData = Model.AnalysisByTag.GetMinutesByProductivity();

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
}
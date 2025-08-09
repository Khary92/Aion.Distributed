using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Proto;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTicketViewModel(AnalysisByTicketModel analysisByTicketModel) : ReactiveObject
{
    private string _mostRelevantTagsByProductivityText = string.Empty;

    private ObservableCollection<ISeries> _productivityTimeSpentBarChart = [];

    private TicketClientModel? _selectedTicket;

    public AnalysisByTicketModel Model { get; } = analysisByTicketModel;

    public TicketClientModel? SelectedTicket
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
        UpdateMostRelevantTagsByProductivityText();
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

    private void UpdateMostRelevantTagsByProductivityText()
    {
        if (Model.AnalysisByTicket == null) return;

        MostRelevantTagsByProductivityText = Model.GetMarkdownString();
    }
}
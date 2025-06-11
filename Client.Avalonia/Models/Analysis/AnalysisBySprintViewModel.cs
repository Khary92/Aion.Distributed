using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Contract.Decorators;
using Contract.DTO;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Client.Avalonia.Models.Analysis;

public class AnalysisBySprintViewModel(AnalysisBySprintModel analysisBySprintModel) : ReactiveObject
{
    private string _displayText = string.Empty;

    private SprintDto? _selectedSprint;

    private string _timeSpentByTicketText = string.Empty;

    public AnalysisBySprintModel Model { get; } = analysisBySprintModel;

    public string DisplayText
    {
        get => _displayText;
        set => this.RaiseAndSetIfChanged(ref _displayText, value);
    }

    public SprintDto? SelectedSprint
    {
        get => _selectedSprint;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSprint, value);
            LoadAnalysisBySprintAsync();
        }
    }

    public ObservableCollection<PieSeries<double>> ProductivityByTimeSpentPieChart { get; } = [];

    public ObservableCollection<PieSeries<double>> TimeSpentByTicketPieChart { get; } = [];

    public string TimeSpentByTicketText
    {
        set => this.RaiseAndSetIfChanged(ref _timeSpentByTicketText, value);
    }

    private async void LoadAnalysisBySprintAsync()
    {
        if (SelectedSprint is null) return;

        await Model.SetAnalysisForSprint(SelectedSprint);

        UpdateTimeSpentByTicketText();
        UpdateProductivityByTimeSpentPieChart();
        UpdateTimeSpentByTicketPieChart();
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        DisplayText = Model.GetMarkdownString();
    }

    private void UpdateProductivityByTimeSpentPieChart()
    {
        if (Model.AnalysisBySprint == null) return;

        var percentageData = Model.AnalysisBySprint.GetTimeSpentInProductivity();

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

    private void UpdateTimeSpentByTicketText()
    {
        if (Model.AnalysisBySprint == null) return;

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Minutes spent by ticket for Sprint " +
                             Model.AnalysisBySprint.SprintName);

        stringBuilder.Append("\n\n");

        var minutesSpentByTicket = Model.AnalysisBySprint.GetMinutesSpentByTicket();
        foreach (var keyValue in minutesSpentByTicket)
            stringBuilder.AppendLine(keyValue.Key + ": " + TimeSpan.FromMinutes(keyValue.Value).ToString(@"hh\:mm"));

        TimeSpentByTicketText = stringBuilder.ToString();
    }

    private void UpdateTimeSpentByTicketPieChart()
    {
        if (Model.AnalysisBySprint == null) return;

        var minutesSpentByTicket = Model.AnalysisBySprint.GetMinutesSpentByTicket();

        TimeSpentByTicketPieChart.Clear();

        var random = new Random();

        foreach (var keyValue in minutesSpentByTicket)
            TimeSpentByTicketPieChart.Add(new PieSeries<double>
            {
                Name = keyValue.Key,
                Values = new List<double> { keyValue.Value },
                Fill = new SolidColorPaint(GetRandomColor()),
                DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                DataLabelsSize = 14,
                DataLabelsPosition = PolarLabelsPosition.Middle,
                DataLabelsFormatter = _ => keyValue.Key
            });

        return;

        SKColor GetRandomColor()
        {
            return new SKColor(
                (byte)random.Next(0, 256),
                (byte)random.Next(0, 256),
                (byte)random.Next(0, 256)
            );
        }
    }
}
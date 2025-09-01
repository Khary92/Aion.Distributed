using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTicketModel(IMessenger messenger, IRequestSender requestSender, ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private const int AmountOfTagsShown = 3;

    private AnalysisByTicketDecorator? _analysisByTicket;

    public ObservableCollection<TicketClientModel> Tickets { get; } = [];

    public AnalysisByTicketDecorator? AnalysisByTicket
    {
        get => _analysisByTicket;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTicket, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        Tickets.Clear();
        Tickets.AddRange(await requestSender.Send(new ClientGetAllTicketsRequest()));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, message) =>
        {
            Tickets.Add(message.Ticket);
            await tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);
        });

        messenger.Register<ClientTicketDataUpdatedNotification>(this, async void (_, notification) =>
        {
            var ticket = Tickets.FirstOrDefault(t => t.TicketId == notification.TicketId);

            if (ticket == null)
            {
                await tracer.Ticket.Update.NoAggregateFound(GetType(), notification.TraceId);
                return;
            }

            ticket.Apply(notification);
            await tracer.Ticket.Update.ChangesApplied(GetType(), notification.TraceId);
        });
    }

    public async Task SetAnalysisByTicket(TicketClientModel selectedTicket)
    {
        AnalysisByTicket =
            await requestSender.Send(new ClientGetTicketAnalysisById(selectedTicket.TicketId));
    }

    public string GetMarkdownString()
    {
        if (_analysisByTicket == null) return string.Empty;

        var builder = new StringBuilder();

        builder.AppendLine($"### Overview for {_analysisByTicket.TicketName}");
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

        if (_analysisByTicket!.ProductiveTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }

        var count = 0;
        foreach (var pair in _analysisByTicket!.ProductiveTags.OrderByDescending(kvp => kvp.Value))
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

        if (_analysisByTicket!.NeutralTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }

        var count = 0;
        foreach (var pair in _analysisByTicket!.NeutralTags.OrderByDescending(kvp => kvp.Value))
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

        if (_analysisByTicket!.UnproductiveTags.Count == 0)
        {
            builder.AppendLine("| None | Not available |");
            return builder.ToString();
        }

        var count = 0;
        foreach (var pair in _analysisByTicket!.UnproductiveTags.OrderByDescending(kvp => kvp.Value))
        {
            if (count == AmountOfTagsShown - 1) break;

            builder.AppendLine($"| {pair.Key} | {pair.Value} |");
            count++;
        }

        return builder.ToString();
    }
}
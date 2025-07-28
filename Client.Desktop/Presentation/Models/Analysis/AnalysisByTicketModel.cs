using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Requests.AnalysisData;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTicketModel(IMessenger messenger, IRequestSender requestSender)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
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
        Tickets.AddRange(await requestSender.Send(new GetTicketsWithShowAllSwitchRequestProto { IsShowAll = true }));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, (_, message) => { Tickets.Add(message.Ticket); });

        messenger.Register<ClientTicketDataUpdatedNotification>(this, (_, notification) =>
        {
            var ticket = Tickets.FirstOrDefault(t => t.TicketId == notification.TicketId);

            if (ticket == null) return;

            ticket.Apply(notification);
        });
    }

    public async Task SetAnalysisByTicket(TicketClientModel selectedTicket)
    {
        AnalysisByTicket = await requestSender.Send(new GetTicketAnalysisById
        {
            TicketId = selectedTicket.TicketId.ToString()
        });
    }

    public async Task<TagClientModel> GetTagById(Guid tagId)
    {
        return await requestSender.Send(new GetTagByIdRequestProto
        {
            TagId = tagId.ToString()
        });
    }
}
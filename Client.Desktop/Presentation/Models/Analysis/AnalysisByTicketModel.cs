using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Services.Initializer;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Command.Tickets;
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
        messenger.Register<NewTicketMessage>(this, (_, m) => { Tickets.Add(m.Ticket); });

        messenger.Register<UpdateTicketDataCommandProto>(this, (_, m) =>
        {
            var ticket = Tickets.FirstOrDefault(t => t.TicketId == Guid.Parse(m.TicketId));

            if (ticket == null) return;

            ticket.Apply(m);
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
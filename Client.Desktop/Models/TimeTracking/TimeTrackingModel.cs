using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Notifications.LocalEvents;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Proto.Command.UseCases;
using Proto.Notifications.Sprint;
using Proto.Notifications.Ticket;
using Proto.Notifications.UseCase;
using Proto.Requests.Sprints;
using Proto.Requests.Tickets;
using Proto.Requests.TimeSlots;
using Proto.Requests.UseCase;
using Proto.Requests.WorkDays;
using ReactiveUI;
using ListEx = DynamicData.ListEx;

namespace Client.Desktop.Models.TimeTracking;

public class TimeTrackingModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITimeSlotViewModelFactory timeSlotViewModelFactory) : ReactiveObject
{
    private ObservableCollection<TicketDto> _filteredTickets = [];

    private int _currentViewModelIndex;
    private TicketDto? _selectedTicket;
    private string _selectedTicketName = string.Empty;

    public string SelectedTicketName
    {
        get => _selectedTicketName;
        set => this.RaiseAndSetIfChanged(ref _selectedTicketName, value);
    }

    public TicketDto? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public int CurrentViewModelIndex
    {
        get => _currentViewModelIndex;
        set => this.RaiseAndSetIfChanged(ref _currentViewModelIndex, value);
    }

    public ObservableCollection<TicketDto> FilteredTickets
    {
        get => _filteredTickets;
        set => this.RaiseAndSetIfChanged(ref _filteredTickets, value);
    }

    private ObservableCollection<TimeSlotViewModel> _timeSlotViewModels = [];

    public ObservableCollection<TimeSlotViewModel> TimeSlotViewModels
    {
        get => _timeSlotViewModels;
        set => this.RaiseAndSetIfChanged(ref _timeSlotViewModels, value);
    }

    private ObservableCollection<TicketDto> AllTickets { get; } = [];

    public async Task Initialize()
    {
        var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

        if (currentSprint == null) return;

        FilteredTickets.Clear();
        var tickets = await requestSender.Send(new GetTicketsForCurrentSprintRequestProto());
        ListEx.AddRange(FilteredTickets, tickets);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, m) =>
        {
            AllTickets.Add(m.Ticket);

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) return;

            if (currentSprint.TicketIds.Contains(m.Ticket.TicketId))
                FilteredTickets.Add(m.Ticket);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            var ticketDto = AllTickets.FirstOrDefault(tsv => tsv.TicketId == Guid.Parse(m.TicketId));

            if (ticketDto == null) return;

            ticketDto.Apply(m);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, async void (_, _) => { await Initialize(); });
        messenger.Register<TicketAddedToActiveSprintNotification>(this, async void (_, _) => { await Initialize(); });

        messenger.Register<TimeSlotControlCreatedNotification>(this,
            (_, m) =>
            {
                timeSlotViewModelFactory.Create(Guid.Parse(m.TicketId), Guid.Parse(m.StatisticsDataId),
                    Guid.Parse(m.TimeSlotId));
            });

        messenger.Register<SprintSelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) throw new InvalidOperationException("No active sprint");

            var ticketDtos = await requestSender.Send(new GetAllTicketsRequestProto());
            foreach (var modelTicket in ticketDtos.Where(modelTicket =>
                         modelTicket.SprintIds.Contains(currentSprint.SprintId)))
                FilteredTickets.Add(modelTicket);
        });

        messenger.Register<WorkDaySelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) throw new InvalidOperationException("No active sprint");

            var ticketDtos = await requestSender.Send(new GetAllTicketsRequestProto());

            foreach (var ticket in ticketDtos.Where(modelTicket =>
                         modelTicket.SprintIds.Contains(currentSprint.SprintId)))
                FilteredTickets.Add(ticket);

            TimeSlotViewModels.Clear();
            await LoadTimeSlotViewModels();
        });
    }

    public void TogglePreviousViewModel()
    {
        if (CurrentViewModelIndex <= 0) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex -= 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    public void ToggleNextViewModel()
    {
        if (CurrentViewModelIndex == TimeSlotViewModels.Count - 1) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex += 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    public async Task LoadTimeSlotViewModels()
    {
        TimeSlotViewModels.Clear();

        var selectedWorkDay = await requestSender.Send(new GetSelectedWorkDayRequestProto());
        var timeSlots = await requestSender.Send(new GetTimeSlotsForWorkDayIdRequestProto
        {
            WorkDayId = selectedWorkDay.WorkDayId.ToString()
        });

        foreach (var timeSlotDto in timeSlots)
        {
            var data = await requestSender.Send(new GetTimeSlotControlDataRequestProto
            {
                TimeSlotId = timeSlotDto.TimeSlotId.ToString()
            });

            var timeSlotViewModel = await timeSlotViewModelFactory.Create(Guid.Parse(data.TicketId),
                Guid.Parse(data.StatisticsDataId), Guid.Parse(data.TimeSlotId));
            TimeSlotViewModels.Add(timeSlotViewModel);
        }

        if (TimeSlotViewModels.Any())
        {
            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        }
    }


    public async Task CreateNewTimeSlotViewModel()
    {
        if (SelectedTicket == null) return;

        await commandSender.Send(new CreateTimeSlotControlCommand
        {
            TicketId = SelectedTicket.TicketId.ToString(),
        });
    }
}
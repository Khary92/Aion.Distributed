using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Tracing.Tracing.Tracers;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class DocumentationModel(
    IRequestSender requestSender,
    INoteViewFactory noteViewFactory,
    ITypeCheckBoxViewModelFactory typeCheckBoxViewModelFactory,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationPublisher)
    : ReactiveObject, IInitializeAsync, IMessengerRegistration
{
    private ObservableCollection<NoteViewModel> _allNotes = [];
    private ObservableCollection<TypeCheckBoxViewModel> _allNoteTypes = [];
    private ObservableCollection<TicketClientModel> _allTickets = [];
    private ObservableCollectionExtended<NoteViewModel> _selectedNotes = [];

    private TicketClientModel? _selectedTicket;

    public ObservableCollection<TicketClientModel> AllTickets
    {
        get => _allTickets;
        set => this.RaiseAndSetIfChanged(ref _allTickets, value);
    }

    public ObservableCollection<NoteViewModel> AllNotes
    {
        get => _allNotes;
        set => this.RaiseAndSetIfChanged(ref _allNotes, value);
    }

    public ObservableCollectionExtended<NoteViewModel> SelectedNotes
    {
        get => _selectedNotes;
        set => this.RaiseAndSetIfChanged(ref _selectedNotes, value);
    }

    public ObservableCollection<TypeCheckBoxViewModel> AllNoteTypes
    {
        get => _allNoteTypes;
        set => this.RaiseAndSetIfChanged(ref _allNoteTypes, value);
    }

    public TicketClientModel? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var noteTypeModels = await requestSender.Send(new ClientGetAllNoteTypesRequest());
        var viewModels = noteTypeModels
            .Select(typeCheckBoxViewModelFactory.Create)
            .ToList();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            AllNoteTypes.Clear();
            AllNoteTypes.AddRange(viewModels);
        });

        var ticketClientModels = await requestSender.Send(new ClientGetAllTicketsRequest());

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            AllTickets.Clear();
            AllTickets.AddRange(ticketClientModels);

            if (!AllTickets.Any()) return;

            SelectedTicket = AllTickets[0];
        });

        await UpdateNotesForSelectedTicket();
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Ticket.NewTicketNotificationReceived += HandleNewTicketMessage;
        notificationPublisher.Ticket.TicketDataUpdatedNotificationReceived += HandleClientTicketDataUpdatedNotification;
        notificationPublisher.Note.NewNoteMessageReceived += HandleNewNoteMessage;
        notificationPublisher.Note.ClientNoteUpdatedNotificationReceived += HandleClientNoteUpdatedNotification;
        notificationPublisher.NoteType.NewNoteTypeMessageReceived += HandleNewNoteTypeMessage;
        notificationPublisher.NoteType.ClientNoteTypeNameChangedNotificationReceived +=
            HandleClientNoteTypeNameChangedNotification;
        notificationPublisher.NoteType.ClientNoteTypeColorChangedNotificationReceived +=
            HandleClientNoteTypeColorChangedNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Ticket.NewTicketNotificationReceived -= HandleNewTicketMessage;
        notificationPublisher.Ticket.TicketDataUpdatedNotificationReceived -= HandleClientTicketDataUpdatedNotification;
        notificationPublisher.Note.NewNoteMessageReceived -= HandleNewNoteMessage;
        notificationPublisher.Note.ClientNoteUpdatedNotificationReceived -= HandleClientNoteUpdatedNotification;
        notificationPublisher.NoteType.NewNoteTypeMessageReceived -= HandleNewNoteTypeMessage;
        notificationPublisher.NoteType.ClientNoteTypeNameChangedNotificationReceived -=
            HandleClientNoteTypeNameChangedNotification;
        notificationPublisher.NoteType.ClientNoteTypeColorChangedNotificationReceived -=
            HandleClientNoteTypeColorChangedNotification;
    }

    private async Task HandleClientNoteUpdatedNotification(ClientNoteUpdatedNotification message)
    {
        await tracer.Note.Update.NotificationReceived(GetType(), message.TraceId, message);

        NoteViewModel? noteViewModel = null;

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            noteViewModel = AllNotes.FirstOrDefault(n => n.Note.NoteId == message.NoteId);
        });

        if (noteViewModel == null)
        {
            await tracer.Note.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        var noteType =
            await requestSender.Send(new ClientGetNoteTypeByIdRequest(message.NoteTypeId));

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            noteViewModel.Note.NoteType = noteType;
            noteViewModel.Note.Apply(message);
        });

        await tracer.Note.Update.ChangesApplied(GetType(), message.TraceId);
        await FilterNotes();
    }

    private async Task HandleClientTicketDataUpdatedNotification(ClientTicketDataUpdatedNotification message)
    {
        TicketClientModel? ticketClientModel = null;
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ticketClientModel = AllTickets.FirstOrDefault(t => t.TicketId == message.TicketId);
        });

        if (ticketClientModel == null)
        {
            await tracer.Ticket.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() => { ticketClientModel.Apply(message); });
        await tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    private async Task HandleNewNoteMessage(NewNoteMessage message)
    {
        var noteViewModel = await noteViewFactory.Create(message.Note);

        await Dispatcher.UIThread.InvokeAsync(() => AllNotes.Add(noteViewModel));

        await tracer.Note.Create.AggregateAdded(GetType(), message.Note.NoteTypeId);

        await FilterNotes();
    }

    private async Task HandleNewTicketMessage(NewTicketMessage message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => { AllTickets.Add(message.Ticket); });
        await tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task UpdateNotesForSelectedTicket()
    {
        if (SelectedTicket == null) return;

        var noteModels =
            await requestSender.Send(new ClientGetNotesByTicketIdRequest(SelectedTicket.TicketId, Guid.NewGuid()));

        var noteViewModels = await Task.WhenAll(noteModels.Select(noteViewFactory.Create));

        await Dispatcher.UIThread.InvokeAsync(() =>
            AllNotes = new ObservableCollection<NoteViewModel>(noteViewModels));

        await FilterNotes();
    }

    private async Task FilterNotes()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var selectedTypes = AllNoteTypes
                .Where(opt => opt.IsChecked)
                .Select(opt => opt.NoteTypeId)
                .ToHashSet();

            var filteredNotes = AllNotes
                .Where(n => selectedTypes.Contains(n.Note.NoteTypeId))
                .OrderBy(n => n.Note.TimeStamp)
                .ToList();

            SelectedNotes.Load(filteredNotes);
        });
    }


    private async Task HandleNewNoteTypeMessage(NewNoteTypeMessage message)
    {
        var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(message.NoteType);

        await Dispatcher.UIThread.InvokeAsync(() => AllNoteTypes.Add(typeCheckBoxViewModel));

        await tracer.NoteType.Create.AggregateAdded(GetType(), message.TraceId);
    }

    private async Task HandleClientNoteTypeNameChangedNotification(ClientNoteTypeNameChangedNotification message)
    {
        await tracer.NoteType.ChangeName.NotificationReceived(GetType(), message.TraceId, message);

        TypeCheckBoxViewModel? typeCheckBoxViewModel = null;
        await Dispatcher.UIThread.InvokeAsync(() =>
            typeCheckBoxViewModel = AllNoteTypes.FirstOrDefault(opt => opt.NoteTypeId == message.NoteTypeId));

        if (typeCheckBoxViewModel == null)
        {
            await tracer.NoteType.ChangeName.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() => typeCheckBoxViewModel.NoteType.Apply(message));
    }

    private async Task HandleClientNoteTypeColorChangedNotification(ClientNoteTypeColorChangedNotification message)
    {
        await tracer.NoteType.ChangeColor.NotificationReceived(GetType(), message.TraceId, message);

        TypeCheckBoxViewModel? typeCheckBoxViewModel = null;

        await Dispatcher.UIThread.InvokeAsync(() =>
            typeCheckBoxViewModel = AllNoteTypes.FirstOrDefault(opt => opt.NoteTypeId == message.NoteTypeId));

        if (typeCheckBoxViewModel == null)
        {
            await tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() => typeCheckBoxViewModel.NoteType.Apply(message));
        await tracer.NoteType.ChangeColor.ChangesApplied(GetType(), message.TraceId);
    }
}
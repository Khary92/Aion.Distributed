using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    : ReactiveObject, IInitializeAsync, IEventRegistration
{
    private ObservableCollection<NoteViewModel> _allNotesByTicket = [];
    private ObservableCollection<TypeCheckBoxViewModel> _allNoteTypes = [];
    private ObservableCollection<TicketClientModel> _allTickets = [];
    private ObservableCollectionExtended<NoteViewModel> _notesFilteredByActiveTypes = [];

    private TicketClientModel? _selectedTicket;

    public ObservableCollection<TicketClientModel> AllTickets
    {
        get => _allTickets;
        set => this.RaiseAndSetIfChanged(ref _allTickets, value);
    }

    public ObservableCollection<NoteViewModel> AllNotesByTicket
    {
        get => _allNotesByTicket;
        set => this.RaiseAndSetIfChanged(ref _allNotesByTicket, value);
    }

    public ObservableCollectionExtended<NoteViewModel> NotesFilteredByActiveTypes
    {
        get => _notesFilteredByActiveTypes;
        set => this.RaiseAndSetIfChanged(ref _notesFilteredByActiveTypes, value);
    }

    public ObservableCollection<TypeCheckBoxViewModel> AllNoteTypes
    {
        get => _allNoteTypes;
        set => this.RaiseAndSetIfChanged(ref _allNoteTypes, value);
    }

    public TicketClientModel? SelectedTicket
    {
        get => _selectedTicket;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTicket, value);
            _ = UpdateNotesForSelectedTicket();
        }
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var noteTypeModels = await requestSender.Send(new ClientGetAllNoteTypesRequest());

        var viewModels = noteTypeModels
            .Select(typeCheckBoxViewModelFactory.Create)
            .ToList();

        foreach (var viewModel in viewModels) viewModel.CheckedChanged += (_, args) => FilterNotes();

        AllNoteTypes.Clear();
        AllNoteTypes.AddRange(viewModels);

        var ticketClientModels = await requestSender.Send(new ClientGetAllTicketsRequest());

        AllTickets.Clear();
        AllTickets.AddRange(ticketClientModels);
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

        var noteViewModel = AllNotesByTicket.FirstOrDefault(n => n.Note.NoteId == message.NoteId);

        if (noteViewModel == null)
            //This is a valid thing. The notes are loaded for the selected ticket only.
            return;

        var noteType =
            await requestSender.Send(new ClientGetNoteTypeByIdRequest(message.NoteTypeId));

        noteViewModel.Note.NoteType = noteType;
        noteViewModel.Note.Apply(message);

        await tracer.Note.Update.ChangesApplied(GetType(), message.TraceId);
        FilterNotes();
    }

    private async Task HandleClientTicketDataUpdatedNotification(ClientTicketDataUpdatedNotification message)
    {
        var ticketClientModel = AllTickets.FirstOrDefault(t => t.TicketId == message.TicketId);

        if (ticketClientModel == null)
        {
            await tracer.Ticket.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        ticketClientModel.Apply(message);
        await tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    private async Task HandleNewNoteMessage(NewNoteMessage message)
    {
        var noteViewModel = await noteViewFactory.Create(message.Note);

        AllNotesByTicket.Add(noteViewModel);

        await tracer.Note.Create.AggregateAdded(GetType(), message.TraceId);

        FilterNotes();
    }

    private async Task HandleNewTicketMessage(NewTicketMessage message)
    {
        AllTickets.Add(message.Ticket);
        await tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task UpdateNotesForSelectedTicket()
    {
        AllNotesByTicket.Clear();

        if (SelectedTicket == null) return;

        var noteModels =
            await requestSender.Send(new ClientGetNotesByTicketIdRequest(SelectedTicket.TicketId, Guid.NewGuid()));

        var noteViewModels = await Task.WhenAll(noteModels.Select(noteViewFactory.Create));

        AllNotesByTicket.AddRange(noteViewModels);

        FilterNotes();
    }

    private void FilterNotes()
    {
        NotesFilteredByActiveTypes.Clear();

        var selectedTypes = AllNoteTypes
            .Where(opt => opt.IsChecked)
            .Select(opt => opt.NoteTypeId)
            .ToHashSet();

        var filteredNotes = AllNotesByTicket
            .Where(n => selectedTypes.Contains(n.Note.NoteTypeId))
            .OrderBy(n => n.Note.TimeStamp)
            .ToList();

        NotesFilteredByActiveTypes = new ObservableCollectionExtended<NoteViewModel>(filteredNotes);
    }


    private async Task HandleNewNoteTypeMessage(NewNoteTypeMessage message)
    {
        var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(message.NoteType);

        typeCheckBoxViewModel.CheckedChanged += (_, args) => FilterNotes();

        AllNoteTypes.Add(typeCheckBoxViewModel);

        await tracer.NoteType.Create.AggregateAdded(GetType(), message.TraceId);
    }

    private async Task HandleClientNoteTypeNameChangedNotification(ClientNoteTypeNameChangedNotification message)
    {
        await tracer.NoteType.ChangeName.NotificationReceived(GetType(), message.TraceId, message);

        var typeCheckBoxViewModel = AllNoteTypes.FirstOrDefault(opt => opt.NoteTypeId == message.NoteTypeId);

        if (typeCheckBoxViewModel == null)
        {
            await tracer.NoteType.ChangeName.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        typeCheckBoxViewModel.NoteType.Apply(message);
    }

    private async Task HandleClientNoteTypeColorChangedNotification(ClientNoteTypeColorChangedNotification message)
    {
        await tracer.NoteType.ChangeColor.NotificationReceived(GetType(), message.TraceId, message);

        var typeCheckBoxViewModel = AllNoteTypes.FirstOrDefault(opt => opt.NoteTypeId == message.NoteTypeId);

        if (typeCheckBoxViewModel == null)
        {
            await tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        typeCheckBoxViewModel.NoteType.Apply(message);
        await tracer.NoteType.ChangeColor.ChangesApplied(GetType(), message.TraceId);
    }
}
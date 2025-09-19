using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class DocumentationModel(
    IMessenger messenger,
    IRequestSender requestSender,
    INoteViewFactory noteViewFactory,
    ITypeCheckBoxViewModelFactory typeCheckBoxViewModelFactory,
    ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IMessengerRegistration, IRecipient<NewTicketMessage>,
        IRecipient<ClientTicketDataUpdatedNotification>, IRecipient<NewNoteMessage>,
        IRecipient<ClientNoteUpdatedNotification>, IRecipient<NewNoteTypeMessage>,
        IRecipient<ClientNoteTypeNameChangedNotification>, IRecipient<ClientNoteTypeColorChangedNotification>
{
    private ObservableCollection<NoteViewModel> _allNotes = [];
    private ObservableCollection<TypeCheckBoxViewModel> _allNoteTypes = [];
    private ObservableCollection<TicketClientModel> _allTickets = [];
    private ObservableCollection<NoteViewModel> _selectedNotes = [];

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

    public ObservableCollection<NoteViewModel> SelectedNotes
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

        AllNoteTypes.Clear();

        foreach (var option in noteTypeModels)
        {
            var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(option);

            AllNoteTypes.Add(typeCheckBoxViewModel);

            typeCheckBoxViewModel.WhenAnyValue(x => x.IsChecked)
                .Subscribe(_ => FilterNotes());
        }

        AllTickets.Clear();
        AllTickets.AddRange(await requestSender.Send(new ClientGetAllTicketsRequest()));

        if (!AllTickets.Any()) return;

        SelectedTicket = AllTickets[0];
        await UpdateNotesForSelectedTicket();
    }

    public void RegisterMessenger()
    {
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ClientNoteTypeColorChangedNotification message)
    {
        _ = HandleClientNoteTypeColorChangedNotification(message);
    }

    public void Receive(ClientNoteTypeNameChangedNotification message)
    {
        _ = HandleClientNoteTypeNameChangedNotification(message);
    }

    public async void Receive(ClientNoteUpdatedNotification message)
    {
        try
        {
            await tracer.Note.Update.NotificationReceived(GetType(), message.TraceId, message);

            var noteViewModel = AllNotes.FirstOrDefault(n => n.Note.NoteId == message.NoteId);

            if (noteViewModel == null)
            {
                await tracer.Note.Update.NoAggregateFound(GetType(), message.TraceId);
                return;
            }

            var noteType =
                await requestSender.Send(new ClientGetNoteTypeByIdRequest(message.NoteTypeId));

            noteViewModel.Note.NoteType = noteType;
            noteViewModel.Note.Apply(message);

            await tracer.Note.Update.ChangesApplied(GetType(), message.TraceId);
            FilterNotes();
        }
        catch (Exception e)
        {
            _ = tracer.Note.Update.ExceptionOccured(GetType(), message.TraceId, e);
        }
    }

    public void Receive(ClientTicketDataUpdatedNotification message)
    {
        var ticket = AllTickets.FirstOrDefault(t => t.TicketId == message.TicketId);

        if (ticket is null)
        {
            _ = tracer.Ticket.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        ticket.Apply(message);
        _ = tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    public async void Receive(NewNoteMessage message)
    {
        try
        {
            var noteViewModel = await noteViewFactory.Create(message.Note);

            AllNotes.Add(noteViewModel);

            await tracer.Note.Create.AggregateAdded(GetType(), message.Note.NoteTypeId);
            FilterNotes();
        }
        catch (Exception exception)
        {
            _ = tracer.Note.Create.ExceptionOccured(GetType(), message.Note.NoteTypeId, exception);
        }
    }

    public void Receive(NewNoteTypeMessage message)
    {
        _ = HandleNewNoteTypeMessage(message);
    }

    public void Receive(NewTicketMessage message)
    {
        AllTickets.Add(message.Ticket);
        _ = tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task UpdateNotesForSelectedTicket()
    {
        if (SelectedTicket == null) return;

        var noteModels =
            await requestSender.Send(new ClientGetNotesByTicketIdRequest(SelectedTicket.TicketId, Guid.NewGuid()));

        var noteViewModels = await Task.WhenAll(noteModels.Select(noteViewFactory.Create));

        AllNotes = new ObservableCollection<NoteViewModel>(noteViewModels);

        FilterNotes();
    }

    private void FilterNotes()
    {
        var selectedTypes = AllNoteTypes.Where(opt => opt.IsChecked).Select(opt => opt).ToHashSet();

        var filteredNotes = AllNotes
            .Where(n => selectedTypes.Any(st => st.NoteTypeId == n.Note.NoteTypeId))
            .OrderBy(n => n.Note.TimeStamp)
            .ToList();

        SelectedNotes = new ObservableCollection<NoteViewModel>(filteredNotes);
    }

    private async Task HandleNewNoteTypeMessage(NewNoteTypeMessage message)
    {
        var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(message.NoteType);

        AllNoteTypes.Add(typeCheckBoxViewModel);

        await tracer.NoteType.Create.AggregateAdded(GetType(), message.TraceId);
    }

    private async Task HandleClientNoteTypeNameChangedNotification(ClientNoteTypeNameChangedNotification message)
    {
        await tracer.NoteType.ChangeName.NotificationReceived(GetType(), message.TraceId, message);

        var typeCheckBoxViewModel = AllNoteTypes.FirstOrDefault(opt => opt.NoteTypeId == message.NoteTypeId);

        if (typeCheckBoxViewModel is null)
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

        if (typeCheckBoxViewModel is null)
        {
            await tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        typeCheckBoxViewModel.NoteType.Apply(message);
        await tracer.NoteType.ChangeColor.ChangesApplied(GetType(), message.TraceId);
    }
}
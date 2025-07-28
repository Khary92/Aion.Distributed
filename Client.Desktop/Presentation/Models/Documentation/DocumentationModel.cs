using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Desktop.Services;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Requests.Notes;
using Proto.Requests.NoteTypes;
using Proto.Requests.Tickets;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class DocumentationModel(
    IRequestSender requestSender,
    IMessenger messenger,
    INoteViewFactory noteViewFactory,
    ITypeCheckBoxViewModelFactory typeCheckBoxViewModelFactory,
    ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private ObservableCollection<NoteViewModel> _allNotes = [];
    private ObservableCollection<TicketClientModel> _allTickets = [];
    private ObservableCollection<TypeCheckBoxViewModel> _options = [];
    private ObservableCollection<NoteViewModel> _selectedNotes = [];

    private TicketClientModel? _selectedTicket;

    public ObservableCollection<TicketClientModel> AllTickets
    {
        get => _allTickets;
        set => this.RaiseAndSetIfChanged(ref _allTickets, value);
    }

    private ObservableCollection<NoteViewModel> AllNotes
    {
        get => _allNotes;
        set => this.RaiseAndSetIfChanged(ref _allNotes, value);
    }

    public ObservableCollection<NoteViewModel> SelectedNotes
    {
        get => _selectedNotes;
        set => this.RaiseAndSetIfChanged(ref _selectedNotes, value);
    }

    public ObservableCollection<TypeCheckBoxViewModel> Options
    {
        get => _options;
        set => this.RaiseAndSetIfChanged(ref _options, value);
    }

    public TicketClientModel? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var noteTypeDtos = await requestSender.Send(new GetAllNoteTypesRequestProto());

        Options.Clear();

        foreach (var option in noteTypeDtos)
        {
            var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(option);

            Options.Add(typeCheckBoxViewModel);

            typeCheckBoxViewModel.WhenAnyValue(x => x.IsChecked)
                .Subscribe(_ => FilterNotes());
        }

        AllTickets.Clear();
        AllTickets.AddRange(await requestSender.Send(new GetAllTicketsRequestProto()));

        if (AllTickets.Any()) SelectedTicket = AllTickets[0];
    }

    public void RegisterMessenger()
    {
        RegisterTicketNotifications();
        RegisterNoteTypeNotifications();
        RegisterNoteNotifications();
    }

    public async Task UpdateNotesForSelectedTicket()
    {
        if (SelectedTicket == null) return;

        var noteDtos = await requestSender.Send(new GetNotesByTicketIdRequestProto
        {
            TicketId = SelectedTicket.TicketId.ToString()
        });

        var noteViewModels = await Task.WhenAll(noteDtos.Select(noteViewFactory.Create));

        AllNotes = new ObservableCollection<NoteViewModel>(noteViewModels);

        FilterNotes();
    }

    private void FilterNotes()
    {
        var selectedTypes = Options.Where(opt => opt.IsChecked).Select(opt => opt).ToHashSet();

        var filteredNotes = AllNotes
            .Where(n => selectedTypes.Any(st => st.NoteTypeId == n.Note.NoteTypeId))
            .OrderBy(n => n.Note.TimeStamp)
            .ToList();

        SelectedNotes = new ObservableCollection<NoteViewModel>(filteredNotes);
    }

    private void RegisterNoteNotifications()
    {
        messenger.Register<NewNoteMessage>(this, async void (_, m) =>
        {
            try
            {
                await tracer.Note.Create.AggregateReceived(GetType(), m.Note.NoteTypeId, m.Note.AsTraceAttributes());

                var noteViewModel = await noteViewFactory.Create(m.Note);

                AllNotes.Add(noteViewModel);

                await tracer.Note.Create.AggregateAdded(GetType(), m.Note.NoteTypeId);
                FilterNotes();
            }
            catch (Exception exception)
            {
                await tracer.Note.Create.ExceptionOccured(GetType(), m.Note.NoteTypeId, exception);
            }
        });

        messenger.Register<ClientNoteUpdatedNotification>(this, async void (_, notification) =>
        {
            try
            {
                await tracer.Note.Update.NotificationReceived(GetType(), notification.NoteTypeId, notification);

                var noteViewModel = AllNotes.FirstOrDefault(n => n.Note.NoteId == notification.NoteTypeId);

                if (noteViewModel == null)
                {
                    await tracer.Note.Update.NoAggregateFound(GetType(), notification.NoteTypeId);
                    return;
                }

                var noteType = await requestSender.Send(new GetNoteTypeByIdRequestProto
                {
                    NoteTypeId = notification.NoteTypeId.ToString()
                });

                noteViewModel.Note.NoteType = noteType;
                noteViewModel.Note.Apply(notification);

                await tracer.Note.Update.ChangesApplied(GetType(), notification.NoteTypeId);
                FilterNotes();
            }
            catch (Exception e)
            {
                await tracer.Note.Update.ExceptionOccured(GetType(), notification.NoteTypeId, e);
            }
        });
    }

    private void RegisterNoteTypeNotifications()
    {
        messenger.Register<NewNoteTypeMessage>(this, async void (_, message) =>
        {
            await tracer.NoteType.Create.AggregateReceived(GetType(), message.NoteType.NoteTypeId,
                message.NoteType.AsTraceAttributes());

            var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(message.NoteType);

            Options.Add(typeCheckBoxViewModel);

            await tracer.NoteType.Create.AggregateAdded(GetType(), message.NoteType.NoteTypeId);
        });

        messenger.Register<ClientNoteTypeNameChangedNotification>(this, async void (_, notification) =>
        {
            await tracer.NoteType.ChangeName.NotificationReceived(GetType(), notification.NoteTypeId, notification);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == notification.NoteTypeId);

            if (typeCheckBoxViewModel is null)
            {
                await tracer.NoteType.ChangeName.NoAggregateFound(GetType(), notification.NoteTypeId);
                return;
            }

            typeCheckBoxViewModel.NoteType!.Apply(notification);
        });

        messenger.Register<ClientNoteTypeColorChangedNotification>(this, (_, notification) =>
        {
            tracer.NoteType.ChangeColor.NotificationReceived(GetType(), notification.NoteTypeId, notification);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == notification.NoteTypeId);

            if (typeCheckBoxViewModel is null)
            {
                tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), notification.NoteTypeId);
                return;
            }

            typeCheckBoxViewModel.NoteType.Apply(notification);
            tracer.NoteType.ChangeColor.ChangesApplied(GetType(), notification.NoteTypeId);
        });
    }

    private void RegisterTicketNotifications()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, m) =>
        {
            await tracer.Ticket.Create.AggregateReceived(GetType(), m.Ticket.TicketId, m.Ticket.AsTraceAttributes());
            AllTickets.Add(m.Ticket);
            await tracer.Ticket.Create.AggregateAdded(GetType(), m.Ticket.TicketId);
        });

        messenger.Register<ClientTicketDataUpdatedNotification>(this, async void (_, notification) =>
        {
            await tracer.Ticket.Update.NotificationReceived(GetType(), notification.TicketId, notification);

            var ticket = AllTickets.FirstOrDefault(t => t.TicketId == notification.TicketId);

            if (ticket is null)
            {
                await tracer.Ticket.Update.NoAggregateFound(GetType(), notification.TicketId);
                return;
            }

            ticket.Apply(notification);
            await tracer.Ticket.Update.ChangesApplied(GetType(), notification.TicketId);
        });
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Desktop.Factories;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Client.Desktop.Services.Initializer;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Notifications.Note;
using Proto.Notifications.NoteType;
using Proto.Notifications.Ticket;
using Proto.Requests.Notes;
using Proto.Requests.NoteTypes;
using Proto.Requests.Tickets;
using ReactiveUI;

namespace Client.Desktop.Models.Documentation;

public class DocumentationModel(
    IRequestSender requestSender,
    IMessenger messenger,
    INoteViewFactory noteViewFactory,
    ITypeCheckBoxViewModelFactory typeCheckBoxViewModelFactory,
    ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private ObservableCollection<NoteViewModel> _allNotes = [];
    private ObservableCollection<TicketDto> _allTickets = [];
    private ObservableCollection<TypeCheckBoxViewModel> _options = [];
    private ObservableCollection<NoteViewModel> _selectedNotes = [];

    private TicketDto? _selectedTicket;

    public ObservableCollection<TicketDto> AllTickets
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

    public TicketDto? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
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
    
    public void RegisterMessenger()
    {
        RegisterTicketNotifications();
        RegisterNoteTypeNotifications();
        RegisterNoteNotifications();
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

        messenger.Register<NoteUpdatedNotification>(this, async void (_, m) =>
        {
            var parsedNoteId = Guid.Parse(m.NoteId);
            try
            {
                await tracer.Note.Update.NotificationReceived(GetType(), parsedNoteId, m);

                var noteViewModel = AllNotes.FirstOrDefault(n => n.Note.NoteId == parsedNoteId);

                if (noteViewModel == null)
                {
                    await tracer.Note.Update.NoAggregateFound(GetType(), parsedNoteId);
                    return;
                }

                var noteType = await requestSender.Send(new GetNoteTypeByIdRequestProto
                {
                    NoteTypeId = m.NoteTypeId
                });

                noteViewModel.Note.NoteType = noteType;
                noteViewModel.Note.Apply(m);

                await tracer.Note.Update.ChangesApplied(GetType(), parsedNoteId);
                FilterNotes();
            }
            catch (Exception e)
            {
                await tracer.Note.Update.ExceptionOccured(GetType(), parsedNoteId, e);
            }
        });
    }

    private void RegisterNoteTypeNotifications()
    {
        messenger.Register<NewNoteTypeMessage>(this, async void (_, m) =>
        {
            await tracer.NoteType.Create.AggregateReceived(GetType(), m.NoteType.NoteTypeId,
                m.NoteType.AsTraceAttributes());

            var typeCheckBoxViewModel = typeCheckBoxViewModelFactory.Create(m.NoteType);
            
            Options.Add(typeCheckBoxViewModel);

            await tracer.NoteType.Create.AggregateAdded(GetType(), m.NoteType.NoteTypeId);
        });

        messenger.Register<NoteTypeNameChangedNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.NoteTypeId);
            await tracer.NoteType.ChangeName.NotificationReceived(GetType(), parsedId, m);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == parsedId);

            if (typeCheckBoxViewModel is null)
            {
                await tracer.NoteType.ChangeName.NoAggregateFound(GetType(), parsedId);
                return;
            }

            typeCheckBoxViewModel.NoteType!.Apply(m);
        });

        messenger.Register<NoteTypeColorChangedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.NoteTypeId);
            tracer.NoteType.ChangeColor.NotificationReceived(GetType(), parsedId, m);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == parsedId);

            if (typeCheckBoxViewModel is null)
            {
                tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), parsedId);
                return;
            }

            typeCheckBoxViewModel.NoteType!.Apply(m);
            tracer.NoteType.ChangeColor.ChangesApplied(GetType(), parsedId);
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

        messenger.Register<TicketDataUpdatedNotification>(this, async void (_, m) =>
        {
            var parsedId = Guid.Parse(m.TicketId);
            await tracer.Ticket.Update.NotificationReceived(GetType(), parsedId, m);

            var ticket = AllTickets.FirstOrDefault(t => t.TicketId == parsedId);

            if (ticket is null)
            {
                await tracer.Ticket.Update.NoAggregateFound(GetType(), parsedId);
                return;
            }

            ticket.Apply(m);
            await tracer.Ticket.Update.ChangesApplied(GetType(), parsedId);
        });
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
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.Notes;
using Client.Avalonia.Communication.Notifications.NoteType;
using Client.Avalonia.Communication.Notifications.Ticket;
using Client.Avalonia.Factories;
using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Note;
using Contract.CQRS.Notifications.Entities.NoteType;
using Contract.CQRS.Notifications.Entities.Tickets;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Documentation;

public class DocumentationModel(
    IMediator mediator,
    IMessenger messenger,
    INoteViewFactory noteViewFactory,
    ITracingCollectorProvider tracer)
    : ReactiveObject
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

        var noteDtos = await mediator.Send(new GetNotesByTicketIdRequest(SelectedTicket.TicketId));

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

    public async Task Initialize()
    {
        var noteTypeDtos = await mediator.Send(new GetAllNoteTypesRequest());

        Options.Clear();

        foreach (var option in noteTypeDtos.Select(type => new TypeCheckBoxViewModel(mediator)
                 {
                     IsChecked = false,
                     NoteTypeId = type.NoteTypeId
                 }))
        {
            Options.Add(option);

            option.WhenAnyValue(x => x.IsChecked)
                .Subscribe(_ => FilterNotes());
        }

        AllTickets.Clear();
        AllTickets.AddRange(await mediator.Send(new GetAllTicketsRequest()));

        if (AllTickets.Any()) SelectedTicket = AllTickets[0];
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
                tracer.Note.Create.AggregateReceived(GetType(), m.Note.NoteTypeId, m.Note.AsTraceAttributes());

                var noteViewModel = await noteViewFactory.Create(m.Note);

                AllNotes.Add(noteViewModel);

                tracer.Note.Create.AggregateAdded(GetType(), m.Note.NoteTypeId);
                FilterNotes();
            }
            catch (Exception exception)
            {
                tracer.Note.Create.ExceptionOccured(GetType(), m.Note.NoteTypeId, exception);
            }
        });

        messenger.Register<NoteUpdatedNotification>(this, async void (_, m) =>
        {
            try
            {
                tracer.Note.Update.NotificationReceived(GetType(), m.NoteTypeId, m);

                var noteViewModel = AllNotes.FirstOrDefault(n => n.Note.NoteId == m.NoteId);

                if (noteViewModel == null)
                {
                    tracer.Note.Update.NoAggregateFound(GetType(), m.NoteTypeId);
                    return;
                }

                var noteType = await mediator.Send(new GetNoteTypeByIdRequest(m.NoteTypeId));
                noteViewModel.Note.NoteType = noteType;
                noteViewModel.Note.Apply(m);

                tracer.Note.Update.ChangesApplied(GetType(), m.NoteTypeId);
                FilterNotes();
            }
            catch (Exception e)
            {
                tracer.Note.Update.ExceptionOccured(GetType(), m.NoteTypeId, e);
            }
        });
    }

    private void RegisterNoteTypeNotifications()
    {
        messenger.Register<NewNoteTypeMessage>(this, (_, m) =>
        {
            tracer.NoteType.Create.AggregateReceived(GetType(), m.NoteType.NoteTypeId, m.NoteType.AsTraceAttributes());

            Options.Add(new TypeCheckBoxViewModel(mediator)
            {
                NoteTypeId = m.NoteType.NoteTypeId,
                NoteType = m.NoteType,
                IsChecked = false
            });

            tracer.NoteType.Create.AggregateAdded(GetType(), m.NoteType.NoteTypeId);
        });

        messenger.Register<NoteTypeNameChangedNotification>(this, (_, m) =>
        {
            tracer.NoteType.ChangeName.NotificationReceived(GetType(), m.NoteTypeId, m);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == m.NoteTypeId);

            if (typeCheckBoxViewModel is null)
            {
                tracer.NoteType.ChangeName.NoAggregateFound(GetType(), m.NoteTypeId);
                return;
            }

            typeCheckBoxViewModel.NoteType!.Apply(m);
        });

        messenger.Register<NoteTypeColorChangedNotification>(this, (_, m) =>
        {
            tracer.NoteType.ChangeColor.NotificationReceived(GetType(), m.NoteTypeId, m);

            var typeCheckBoxViewModel = Options.FirstOrDefault(opt => opt.NoteTypeId == m.NoteTypeId);

            if (typeCheckBoxViewModel is null)
            {
                tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), m.NoteTypeId);
                return;
            }

            typeCheckBoxViewModel.NoteType!.Apply(m);
            tracer.NoteType.ChangeColor.ChangesApplied(GetType(), m.NoteTypeId);
        });
    }

    private void RegisterTicketNotifications()
    {
        messenger.Register<NewTicketMessage>(this, (_, m) =>
        {
            tracer.Ticket.Create.AggregateReceived(GetType(), m.Ticket.TicketId, m.Ticket.AsTraceAttributes());
            AllTickets.Add(m.Ticket);
            tracer.Ticket.Create.AggregateAdded(GetType(), m.Ticket.TicketId);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            tracer.Ticket.Update.NotificationReceived(GetType(), m.TicketId, m);

            var ticket = AllTickets.FirstOrDefault(t => t.TicketId == m.TicketId);

            if (ticket is null)
            {
                tracer.Ticket.Update.NoAggregateFound(GetType(), m.TicketId);
                return;
            }

            ticket.Apply(m);
            tracer.Ticket.Update.ChangesApplied(GetType(), m.TicketId);
        });
    }
}
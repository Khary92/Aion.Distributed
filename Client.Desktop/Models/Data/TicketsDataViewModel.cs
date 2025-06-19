using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.Sprints;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Data;

public class TicketsDataViewModel : ReactiveObject
{
   // private readonly ITracingCollectorProvider _tracer;
    private string _editButtonText = string.Empty;

    private bool _isEditMode;

    private bool _isShowAllTicketsActive;

    private string _newTicketBookingNumber = string.Empty;

    private string _newTicketName = string.Empty;

    private TicketDto? _selectedTicket;

    public TicketsDataViewModel(IMessenger messenger, TicketsDataModel ticketsDataModel)
    {
        DataModel = ticketsDataModel;

        EditTicketCommand = ReactiveCommand.Create(ToggleTagEditMode);

        CreateTicketCommand = ReactiveCommand.CreateFromTask(PersistTicket,
            this.WhenAnyValue(x => x.NewTicketName,
                x => x.NewTicketBookingNumber,
                (name, booking) =>
                    !string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(booking))
        );

        AddTicketToCurrentSprintCommand = ReactiveCommand.CreateFromTask(AddTicketToActiveSprint,
            this.WhenAnyValue(x => x.SelectedTicket).Any()
        );

        messenger.Register<SetSprintActiveStatusCommandProto>(this,
            async void (_, _) => { await DataModel.InitializeAsync(IsShowAllTicketsActive); });

        IsEditMode = false;

        DataModel.InitializeAsync(IsShowAllTicketsActive).ConfigureAwait(false);
        DataModel.RegisterMessenger();
    }

    public TicketsDataModel DataModel { get; }

    public string EditButtonText
    {
        get => _editButtonText;
        set => this.RaiseAndSetIfChanged(ref _editButtonText, value);
    }

    private bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            this.RaiseAndSetIfChanged(ref _isEditMode, value);
            EditButtonText = _isEditMode ? "Cancel Edit" : "Edit";
        }
    }

    public TicketDto? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public string NewTicketName
    {
        get => _newTicketName;
        set => this.RaiseAndSetIfChanged(ref _newTicketName, value);
    }

    public string NewTicketBookingNumber
    {
        get => _newTicketBookingNumber;
        set => this.RaiseAndSetIfChanged(ref _newTicketBookingNumber, value);
    }

    public bool IsShowAllTicketsActive
    {
        get => _isShowAllTicketsActive;
        set
        {
            this.RaiseAndSetIfChanged(ref _isShowAllTicketsActive, value);
            _ = DataModel.InitializeAsync(IsShowAllTicketsActive);
        }
    }

    public ReactiveCommand<Unit, Unit> CreateTicketCommand { get; }
    public ReactiveCommand<Unit, Unit> EditTicketCommand { get; }
    public ReactiveCommand<Unit, Unit> AddTicketToCurrentSprintCommand { get; }

    private void ToggleTagEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode) return;

        NewTicketName = SelectedTicket != null ? SelectedTicket.Name : string.Empty;
        NewTicketBookingNumber = SelectedTicket != null ? SelectedTicket.BookingNumber : string.Empty;
    }

    private async Task AddTicketToActiveSprint()
    {
        //_tracer.Ticket.AddTicketToSprint.StartUseCase(GetType(), SelectedTicket!.TicketId,
            //SelectedTicket.AsTraceAttributes());

        await DataModel.AddTicketToCurrentSprint(SelectedTicket!);
        ResetData();
    }

    private async Task PersistTicket()
    {
        if (IsEditMode)
        {
            var updatedTicket = new TicketDto(SelectedTicket!.TicketId, NewTicketName, NewTicketBookingNumber,
                SelectedTicket.Documentation, SelectedTicket.SprintIds);

           // _tracer.Ticket.Update.StartUseCase(GetType(), SelectedTicket!.TicketId, updatedTicket.AsTraceAttributes());

            await DataModel.UpdateTicket(updatedTicket);

            IsEditMode = false;
            ResetData();
            return;
        }

        var createTicketDto = new TicketDto(Guid.NewGuid(), NewTicketName, NewTicketBookingNumber, string.Empty, []);

        //_tracer.Ticket.Create.StartUseCase(GetType(), createTicketDto.TicketId, createTicketDto.AsTraceAttributes());

        await DataModel.CreateTicket(createTicketDto);
        ResetData();
    }

    private void ResetData()
    {
        NewTicketName = string.Empty;
        NewTicketBookingNumber = string.Empty;
    }
}
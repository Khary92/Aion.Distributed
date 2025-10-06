using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Command.Tickets;
using Proto.DTO.TraceData;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerTicketDataViewModel : ReactiveObject
{
    private string _editButtonText = string.Empty;

    private bool _isEditMode;

    private string _newTicketBookingNumber = string.Empty;

    private string _newTicketName = string.Empty;

    private TicketClientModel? _selectedTicket;

    public ServerTicketDataViewModel(ServerTicketDataModel serverTicketDataModel)
    {
        DataModel = serverTicketDataModel;

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

        IsEditMode = false;
    }

    public ServerTicketDataModel DataModel { get; }

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

    public TicketClientModel? SelectedTicket
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
        // TODO This is not done here...
        ResetData();
    }

    private async Task PersistTicket()
    {
        if (IsEditMode)
        {
            await DataModel.Send(new UpdateTicketDataCommandProto()
            {
                TicketId = SelectedTicket!.TicketId.ToString(),
                Name = NewTicketName,
                BookingNumber = NewTicketBookingNumber,
                SprintIds = { SelectedTicket!.SprintIds.ToRepeatedField() },
                TraceData = new TraceDataProto()
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            });

            ResetData();
            return;
        }

        await DataModel.Send(new CreateTicketCommandProto()
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = NewTicketName,
            BookingNumber = NewTicketBookingNumber,
            SprintIds = { },
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        });
        ResetData();
    }

    private void ResetData()
    {
        NewTicketName = string.Empty;
        NewTicketBookingNumber = string.Empty;
    }
}
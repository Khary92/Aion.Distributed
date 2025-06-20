using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Models.TimeTracking;
using Client.Desktop.Replays;
using Client.Desktop.Views.Custom;
using Microsoft.Extensions.DependencyInjection;
using Proto.Requests.StatisticsData;
using Proto.Requests.Tickets;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Factories;

public class TimeSlotViewModelFactory(IServiceProvider serviceProvider, IRequestSender requestSender)
    : ITimeSlotViewModelFactory
{
    public async Task<TimeSlotViewModel> Create(Guid ticketId, Guid statisticsDataId, Guid timeSlotId)
    {
        var timeSlotViewModel = serviceProvider.GetRequiredService<TimeSlotViewModel>();

        var ticketDto = await requestSender.Send(new GetTicketByIdRequestProto
        {
            TicketId = ticketId.ToString()
        });

        var replayDecorator = new TicketReplayDecorator(requestSender, ticketDto)
        {
            DisplayedDocumentation = ticketDto.Documentation,
            IsReplayMode = false
        };

        timeSlotViewModel.Model.TicketReplayDecorator = replayDecorator;

        var timeSlotDto = await requestSender.Send(new GetTimeSlotByIdRequestProto
        {
            TimeSlotId = timeSlotId.ToString()
        });

        timeSlotViewModel.Model.TimeSlot = timeSlotDto;

        var statisticsDataDto = await requestSender.Send(new GetStatisticsDataByTimeSlotIdRequestProto
        {
            TimeSlotId = timeSlotDto.TimeSlotId.ToString()
        });

        timeSlotViewModel.StatisticsViewModel.StatisticsData = statisticsDataDto;

        await timeSlotViewModel.StatisticsViewModel.Initialize();
        timeSlotViewModel.StatisticsViewModel.RegisterMessenger();

        timeSlotViewModel.NoteStreamViewModel.TimeSlotId = timeSlotId;
        await timeSlotViewModel.NoteStreamViewModel.InitializeAsync();
        timeSlotViewModel.NoteStreamViewModel.RegisterMessenger();

        timeSlotViewModel.InitializeViewTimer(new ViewTimer());
        return timeSlotViewModel;
    }
}
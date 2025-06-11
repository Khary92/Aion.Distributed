using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Avalonia.Communication.RequiresChange;
using Client.Avalonia.FileSystem;
using Contract.CQRS.Requests.Settings;
using Contract.DTO;
using MediatR;

namespace Application.Services.UseCase;

public class ExportService(
    IMediator mediator,
    //ITimeSlotRequestsService timeSlotRequestsService,
    //ITicketRequestsService ticketRequestsService,
    IFileSystemWriter fileSystemWriter)
    : IExportService
{
    public async Task<bool> ExportToFile(Collection<WorkDayDto> workDayDtos)
    {
        if (workDayDtos.Count == 0) return false;

        var markdownString = await GetMarkdownString(workDayDtos);
        var config = await mediator.Send(new GetSettingsRequest());
        var filePath = BuildFilePath(workDayDtos.First().Date.Date, config!.ExportPath);

        try
        {
            await fileSystemWriter.Write(markdownString, filePath);
        }
        catch (IOException e)
        {
            return false;
        }

        return true;
    }

    public async Task<string> GetMarkdownString(Collection<WorkDayDto> workDayDtos)
    {
        var exportDataHolders = await GetDataForSelectedWorkDays(workDayDtos);
        var builder = new StringBuilder();

        foreach (var dataHolder in exportDataHolders)
        {
            builder.AppendLine($"### Project times for {dataHolder.Key.Date.ToShortDateString()}");
            builder.AppendLine("| Ticket | Booking number | Time |");
            builder.AppendLine("| ------- | --------------- | ----- |");

            foreach (var ticketData in dataHolder.Value)
                builder.AppendLine(
                    $"| {ticketData.Name} | {ticketData.BookingNumber} | {ticketData.TotalTime()} |");
        }

        return builder.ToString();
    }

    private static string BuildFilePath(DateTime date, string exportPath)
    {
        return Path.Combine(exportPath, $"{date:MM/dd/yyyy}.md");
    }

    private async Task<Dictionary<DateTimeOffset, List<TicketDataHolder>>> GetDataForSelectedWorkDays(
        Collection<WorkDayDto> workDayDtos)
    {
        var result = new Dictionary<DateTimeOffset, List<TicketDataHolder>>();

       // var domainWorkDays = workDayDtos.Select(workDayMapper.ToDomain).ToList();
        //foreach (var workDay in domainWorkDays) await FillDictionaryForWorkday(workDay, result);

        return result;
    }

    private async Task FillDictionaryForWorkday(WorkDayDto workDay,
        Dictionary<DateTimeOffset, List<TicketDataHolder>> result)
    {
        var timeSlots = new List<TimeSlotDto>();//await timeSlotRequestsService.GetTimeSlotsForWorkDayId(workDay.WorkDayId);
        if (timeSlots.Count == 0) return;

        foreach (var timeSlot in timeSlots)
        {
            TicketDto ticket = null;//await ticketRequestsService.GetTicketAsync(timeSlot.SelectedTicketId);
            var elapsedSeconds = (int)(timeSlot.EndTime - timeSlot.StartTime).TotalSeconds;

            if (!result.TryGetValue(workDay.Date, out var ticketDataList))
            {
                ticketDataList = [];
                result.Add(workDay.Date, ticketDataList);
            }

            var existingData = ticketDataList.FirstOrDefault(t => t.TicketId == ticket!.TicketId);
            if (existingData != null)
            {
                existingData.ElapsedSeconds += elapsedSeconds;
                continue;
            }

            ticketDataList.Add(new TicketDataHolder(ticket!.TicketId, ticket.Name, ticket.BookingNumber,
                elapsedSeconds));
        }
    }

    private class TicketDataHolder(
        Guid ticketId,
        string name,
        string bookingNumber,
        int elapsedSeconds)
    {
        public Guid TicketId { get; } = ticketId;
        public string Name { get; } = name;
        public string BookingNumber { get; } = bookingNumber;
        public int ElapsedSeconds { get; set; } = elapsedSeconds;

        public string TotalTime()
        {
            return TimeSpan.FromSeconds(ElapsedSeconds).ToString(@"hh\:mm") + "h";
        }
    }
}
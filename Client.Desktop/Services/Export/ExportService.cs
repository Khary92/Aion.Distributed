using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Local;
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Services.Export;

public class ExportService(
    IRequestSender requestSender,
    IFileSystemWriter fileSystemWriter,
    IMessenger messenger)
    : IExportService, IRegisterMessenger
{
    private SettingsClientModel? LocalSettings { get; set; }

    public async Task<bool> ExportToFile(Collection<WorkDayClientModel> workDayDtos)
    {
        if (workDayDtos.Count == 0) return false;

        var markdownString = await GetMarkdownString(workDayDtos);
        var filePath = BuildFilePath(workDayDtos.First().Date.Date, LocalSettings!.ExportPath);

        try
        {
            await fileSystemWriter.Write(markdownString, filePath);
        }
        catch (IOException ex)
        {
            Console.WriteLine(GetType() + " caused an exception: " + ex);
            return false;
        }

        return true;
    }

    public async Task<string> GetMarkdownString(Collection<WorkDayClientModel> workDayDtos)
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

    public void RegisterMessenger()
    {
        messenger.Register<ExportPathSetNotification>(this,
            (_, message) => { LocalSettings!.ExportPath = message.ExportPath; });

        messenger.Register<SettingsClientModel>(this, (_, message) => { LocalSettings = message; });
    }

    private static string BuildFilePath(DateTime date, string exportPath)
    {
        return Path.Combine(exportPath, $"{date:MM-dd-yyyy}.md");
    }

    private async Task<Dictionary<DateTimeOffset, List<TicketDataHolder>>> GetDataForSelectedWorkDays(
        Collection<WorkDayClientModel> workDayDtos)
    {
        var result = new Dictionary<DateTimeOffset, List<TicketDataHolder>>();

        foreach (var workDay in workDayDtos) await FillDictionaryForWorkday(workDay, result);

        return result;
    }

    private async Task FillDictionaryForWorkday(WorkDayClientModel workDay,
        Dictionary<DateTimeOffset, List<TicketDataHolder>> result)
    {
        var timeSlots =
            await requestSender.Send(new ClientGetTimeSlotsForWorkDayIdRequest(workDay.WorkDayId));

        if (timeSlots.Count == 0) return;

        foreach (var timeSlot in timeSlots)
        {
            var ticket =
                await requestSender.Send(new ClientGetTicketByIdRequest(timeSlot.SelectedTicketId));

            var elapsedSeconds = (int)(timeSlot.EndTime - timeSlot.StartTime).TotalSeconds;

            if (!result.TryGetValue(workDay.Date, out var ticketDataList))
            {
                ticketDataList = [];
                result.Add(workDay.Date, ticketDataList);
            }

            var existingData = ticketDataList.FirstOrDefault(t => t.TicketId == ticket.TicketId);
            if (existingData != null)
            {
                existingData.ElapsedSeconds += elapsedSeconds;
                continue;
            }

            ticketDataList.Add(new TicketDataHolder(ticket.TicketId, ticket.Name, ticket.BookingNumber,
                elapsedSeconds));
        }
    }

    private class TicketDataHolder(Guid ticketId, string name, string bookingNumber, int elapsedSeconds)
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
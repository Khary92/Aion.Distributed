using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Export;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Export;

public static class ExportModelProvider
{
    public sealed class ExportModelFixture
    {
        public required ExportModel Instance { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<IExportService> ExportService { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
    }

    private static IMessenger CreateMessenger() => new WeakReferenceMessenger();

    private static Mock<ITraceCollector> CreateTracerMock()
        => new() { DefaultValue = DefaultValue.Mock };

    private static Mock<IRequestSender> CreateRequestSenderMock() => new();
    private static Mock<ILocalSettingsService> CreateLocalSettingsServiceMock() => new();
    private static Mock<IExportService> CreateExportServiceMock() => new();

    public static async Task<ExportModelFixture> Create(List<WorkDayClientModel> initialWorkDays)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var localSettingsService = CreateLocalSettingsServiceMock();
        var exportService = CreateExportServiceMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllWorkDaysRequest>()))
            .ReturnsAsync(initialWorkDays);

        return await CreateFixture(messenger, requestSender, tracer, localSettingsService, exportService);
    }

    private static async Task<ExportModelFixture> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ITraceCollector> tracer,
        Mock<ILocalSettingsService> localSettingsService, Mock<IExportService> exportService)
    {
        var instance = new ExportModel(messenger, requestSender.Object, exportService.Object, tracer.Object,
            localSettingsService.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();


        return new ExportModelFixture()
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            Messenger = messenger,
            LocalSettingsService = localSettingsService,
            ExportService = exportService,
        };
    }
}
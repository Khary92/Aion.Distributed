using System;

namespace Client.Desktop.Communication.Requests.StatisticsData.Records;

public record ClientGetStatisticsDataByTimeSlotIdRequest(Guid TimeSlotId);
using System;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.DTO.StatisticsData;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public static class StatisticsDataRequestExtensions
{
    public static GetStatisticsDataByTimeSlotIdRequestProto ToProto(
        this ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        return new GetStatisticsDataByTimeSlotIdRequestProto
        {
            TimeSlotId = request.TimeSlotId.ToString()
        };
    }

    public static StatisticsDataClientModel ToClientModel(this StatisticsDataProto proto)
    {
        return new StatisticsDataClientModel(
            Guid.Parse(proto.StatisticsId), Guid.Parse(proto.TimeSlotId), proto.TagIds.ToGuidList(),
            proto.IsProductive, proto.IsNeutral, proto.IsUnproductive);
    }
}
using Service.Monitoring.Shared;

namespace Service.Monitoring.Verifiers.Common;

public static class TraceDataExtensions
{
    public static List<string> GetClassTrace(this List<TraceData> traceData)
    {
        var result = new List<string>();

        foreach (var trace in traceData)
            result.Add(trace.TimeStamp + " | " + trace.OriginClassType + " | " + trace.Log);

        return result;
    }
}
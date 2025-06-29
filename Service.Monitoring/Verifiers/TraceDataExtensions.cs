using Service.Monitoring.Shared;

namespace Service.Monitoring.Verifiers;

public static class TraceDataExtensions
{
    public static string GetClassTrace(this List<TraceData> traceData)
    {
        string result = string.Empty;

        foreach (var trace in traceData)
        {
            result += trace.TimeStamp +  " | " +  trace.OriginClassType + " | " + trace.Log + "\n";
        }

        return result;
    }
}
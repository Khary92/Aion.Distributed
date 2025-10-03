using Global.Settings.Types;
using Global.Settings.UrlResolver.Data;
using Microsoft.Extensions.Options;

namespace Global.Settings.UrlResolver;

public class GrpcUrlBuilder : IGrpcUrlBuilder
{
    private readonly IReadOnlyDictionary<ResolvingServices, IGrpcDataProvider> _serviceData;
    private readonly bool _useHttps;
    private ResolvingServices? _from;
    private ResolvingServices? _to;

    public GrpcUrlBuilder(IEnumerable<IGrpcDataProvider> dataProviders, IOptions<GlobalSettings> globalSettings)
    {
        _serviceData = dataProviders?.ToDictionary(r => r.ResolvingServices)
                       ?? throw new ArgumentNullException(nameof(dataProviders));
        _useHttps = globalSettings?.Value?.UseHttps ?? false;
    }

    public GrpcUrlBuilder From(ResolvingServices from)
    {
        _from = from;
        return this;
    }

    public GrpcUrlBuilder To(ResolvingServices to)
    {
        _to = to;
        return this;
    }

    public string BuildAddress()
    {
        if (_from is null) throw new ArgumentException("From service is null.");
        if (_to is null) throw new ArgumentException("To service is null.");
        if (_from == _to) throw new ArgumentException("From and To may not be the same.");

        if (!_serviceData.TryGetValue(_to.Value, out var target))
            throw new InvalidOperationException($"No data provider for '{_to}'.");

        if (_from == ResolvingServices.Client)
        {
            var host = target.IsRunningInDocker ? target.DockerHostName : target.HostName;
            return ConcatenateData(host, target.DockerGrpcPort);
        }

        if (!_serviceData.TryGetValue(_from.Value, out var origin))
            throw new InvalidOperationException($"No data provider for '{_from}'.");


        if (origin.IsRunningInDocker && target.IsRunningInDocker)
            return ConcatenateData(target.HostName, target.GrpcPort);

        throw new NotImplementedException($"Sending messages From={_from} To={_to} is not supported yet.");
    }

    private string ConcatenateData(string host, int port)
    {
        var scheme = _useHttps ? "https" : "http";

        // TODO this is needed in a singleton scope. But this service is not stateless. Probably use a factory later on.
        _from = null;
        _to = null;

        return $"{scheme}://{host}:{port}";
    }
}
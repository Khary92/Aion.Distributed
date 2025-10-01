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
        if (_from is null) throw new ArgumentException("From service ist null.");
        if (_to is null) throw new ArgumentException("To service ist null.");
        if (_from == _to) throw new ArgumentException("From und To dürfen nicht gleich sein.");

        if (!_serviceData.TryGetValue(_to.Value, out var target))
            throw new InvalidOperationException($"Kein DataProvider für '{_to}'.");

        if (_from == ResolvingServices.Client)
        {
            var host = target.IsRunningInDocker ? target.DockerHostName : target.HostName;
            return ConcatenateData(host, target.DockerGrpcPort);
        }

        if (!_serviceData.TryGetValue(_from.Value, out var origin))
            throw new InvalidOperationException($"Kein DataProvider für '{_from}'.");


        if (origin.IsRunningInDocker && target.IsRunningInDocker)
            return ConcatenateData(target.HostName, target.GrpcPort);

        throw new NotImplementedException($"Konstellation From={_from} To={_to} wird noch nicht unterstützt.");
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
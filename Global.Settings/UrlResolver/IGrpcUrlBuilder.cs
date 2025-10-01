namespace Global.Settings.UrlResolver;

public interface IGrpcUrlBuilder
{
    GrpcUrlBuilder From(ResolvingServices from);
    GrpcUrlBuilder To(ResolvingServices to);
    string BuildAddress();
}
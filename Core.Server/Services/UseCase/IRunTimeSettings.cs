namespace Service.Server.Services.UseCase;

public interface IRunTimeSettings
{
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}
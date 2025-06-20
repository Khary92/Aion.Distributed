namespace Service.Server.Old.Services.UseCase;

public interface IRunTimeSettings
{
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}
namespace Contract.LanguageModel;

public interface ILanguageModelApi
{
    Task ReloadModel();
    void StartRequest(string input);
    void CancelRequest();
    event EventHandler<string> OnResponseReceived;
}
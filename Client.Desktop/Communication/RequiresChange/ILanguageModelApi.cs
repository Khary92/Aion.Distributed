using System;
using System.Threading.Tasks;

namespace Client.Desktop.Communication.RequiresChange;

public interface ILanguageModelApi
{
    Task ReloadModel();
    void StartRequest(string input);
    void CancelRequest();
    event EventHandler<string> OnResponseReceived;
}
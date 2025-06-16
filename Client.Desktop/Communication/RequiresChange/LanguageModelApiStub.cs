using System;
using System.Threading.Tasks;

namespace Client.Desktop.Communication.RequiresChange;

public class LanguageModelApiStub : ILanguageModelApi
{
    public Task ReloadModel()
    {
        throw new NotImplementedException();
    }

    public void StartRequest(string input)
    {
        throw new NotImplementedException();
    }

    public void CancelRequest()
    {
        throw new NotImplementedException();
    }

    public event EventHandler<string>? OnResponseReceived;
}
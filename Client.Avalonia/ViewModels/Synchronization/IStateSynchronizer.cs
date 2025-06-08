using System;
using System.Threading.Tasks;

namespace Client.Avalonia.ViewModels.Synchronization;

public interface IStateSynchronizer<in TObject, in TValue>
{
    void Register(Guid id, TObject obj);
    void SetSynchronizationValue(Guid id, TValue value);
    Task FireCommand();
}
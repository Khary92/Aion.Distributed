using System;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Sender;

namespace Client.Avalonia.Communication.RequiresChange.Cache;

public class GrpcCommandSender : ICommandSender
{
    public Task Send<T>(T command)
    {
        throw new NotImplementedException();
    }
}
using System;
using System.Threading.Tasks;

namespace Chat.Service
{
  public sealed class Client : IDisposable
  {
    public bool IsAuthenticated { get; }

    public Task<string> SendPasswordRequestAsync() => Task.FromResult((string)null);

    public void Dispose()
    {
    }
  }
}
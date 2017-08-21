using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Chat.Service
{
  public sealed class ReactiveProgram
  {
    public bool AuthenticationEnabled { get; set; }

    public IObservable<Client> Clients { private get; set; }

    public IDisposable Start()
    {
      var clients = (from client in Clients
                     select (WhiteListed: IsInWhiteList(client), Client: client))
                    .Publish();

      var whitelisted = from c in clients
                        where c.WhiteListed
                        select c.Client;

      if (AuthenticationEnabled)
      {
        whitelisted = from client in whitelisted
                      from password in (from password in client.SendPasswordRequestAsync().ToObservable()
                                        .Timeout(TimeSpan.FromMinutes(2))
                                        from _ in client.IsAuthenticated
                                                ? Observable.Return(string.Empty)
                                                : Observable.Throw<string>(new Exception("Incorrect password specified."))
                                        select password)
                                       .Catch(TerminateClient(client))
                      select client;
      }

      return StableCompositeDisposable.Create(

        (from c in clients
         where !c.WhiteListed
         select c.Client)
         .Subscribe(client => client.Dispose()),

        whitelisted.Subscribe(),

        clients.Connect());
    }

    private Func<Exception, IObservable<string>> TerminateClient(Client client)
         => ex =>
         {
           client.Dispose();

           // Trace.Error(ex);

           return Observable.Empty<string>();
         };

    private bool IsInWhiteList(Client client)
    {
      throw new NotImplementedException();
    }
  }
}

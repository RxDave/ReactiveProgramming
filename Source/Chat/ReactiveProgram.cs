using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Chat
{
  public sealed class ReactiveProgram
  {
    private readonly IConnectableObservable<User> users;

    public ReactiveProgram(ReactiveInput input)
    {
      Input = input;

      users = (from userName in input.UserNames
               select User.TryCreate(userName))
               .Publish();

      var validUserNames = from user in users
                           where user != null
                           select user;

      InvalidUserNames = from user in users
                         where user == null
                         select Unit.Default;

      // TODO: Implement valid/invalid service URL observables
      // using a tuple rather than a discrete type.
      var validServiceUrls = from url in input.ServiceUrls
                             select url;
    }

    public ReactiveInput Input { get; }

    IObservable<bool> Connections { get; }
    IObservable<ConnectionFailureReason> ConnectionFailures { get; }
    IObservable<Unit> Authentications { get; }

    IObservable<Unit> InvalidUserNames { get; }
    IObservable<Unit> InvalidServiceUrls { get; }

    public void Main()
    {
    }

    public IDisposable Connect() => users.Connect();

    public sealed class ReactiveInput
    {
      public IObservable<string> UserNames { get; set; }
      public IObservable<Uri> ServiceUrls { get; set; }
      public IObservable<Unit> Connects { get; set; }
      public IObservable<Unit> Cancellations { get; set; }
      public IObservable<Unit> Authentications { get; set; }
      public IObservable<string> Passwords { get; set; }
    }
  }
}

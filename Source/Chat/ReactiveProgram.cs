using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static Chat.UriExtensions;

namespace Chat
{
  public sealed class ReactiveProgram
  {
    private readonly IConnectableObservable<User> users;
    private readonly IConnectableObservable<Uri> serviceUrls;

    public ReactiveProgram(ReactiveInput input)
    {
      Input = input;

      users = (from userName in input.UserNames
               select User.TryCreate(userName))
               .Publish();

      serviceUrls = (from url in input.ServiceUrls
                     select TryCreateUri(url))
                     .Publish();

      var validUsers = from user in users
                       where user != null
                       select user;

      var validServiceUrls = from url in serviceUrls
                             where url != null
                             select url;

      var connects = (from validState in validUsers.CombineLatest(validServiceUrls, (user, url) => (User: user, Url: url))
                      select input.ConnectionAttempts.Select(_ => validState))
                      .Switch();

      InvalidUserNames = from user in users
                         where user == null
                         select Unit.Default;

      InvalidServiceUrls = from url in serviceUrls
                           where url == null
                           select Unit.Default;
    }

    public ReactiveInput Input { get; }

    // Presentation Layer
    public IObservable<Unit> InvalidUserNames { get; }
    public IObservable<Unit> InvalidServiceUrls { get; }
    public IObservable<Unit> ServiceAuthentications { get; }
    public IObservable<ConnectionFailureReason> ConnectionFailures { get; }
    public IObservable<bool> ConnectionStates { get; }
    public IObservable<Message> ServiceMessages { get; }

    // Service Layer
    public IObservable<Unit> ConnectionAttempts { get; }
    public IObservable<Unit> ClientAuthentications { get; }
    public IObservable<Unit> Cancellations { get; }
    public IObservable<Message> ClientMessages { get; }
    public IObservable<Unit> Disconnections { get; }

    public void Main()
    {
    }

    public IDisposable Connect() => StableCompositeDisposable.Create(
      users.Connect(),
      serviceUrls.Connect());

    public sealed class ReactiveInput
    {
      // Presentation Layer
      public IObservable<string> UserNames { get; set; }
      public IObservable<string> ServiceUrls { get; set; }
      public IObservable<Unit> ConnectionAttempts { get; set; }
      public IObservable<Unit> Cancellations { get; set; }
      public IObservable<string> Passwords { get; set; }
      public IObservable<Unit> ClientAuthentications { get; set; }
      public IObservable<Message> ClientMessages { get; set; }
      public IObservable<Unit> Disconnections { get; set; }

      // Service Layer
      public IObservable<Unit> ServiceAuthentications { get; }
      public IObservable<ConnectionFailureReason> ConnectionFailures { get; }
      public IObservable<bool> ConnectionStates { get; }
      public IObservable<Message> ServiceMessages { get; set; }
    }
  }
}

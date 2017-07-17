using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
                     select TryCreate(url))
                     .Publish();

      var validUsers = from user in users
                       where user != null
                       select user;

      var validServiceUrls = from url in serviceUrls
                             where url != null
                             select url;

      var connects = (from validState in validUsers.CombineLatest(validServiceUrls, (user, url) => (User: user, Url: url))
                      select input.Connects.Select(_ => validState))
                      .Switch();

      InvalidUserNames = from user in users
                         where user == null
                         select Unit.Default;

      InvalidServiceUrls = from url in serviceUrls
                           where url == null
                           select Unit.Default;
    }

    public ReactiveInput Input { get; }

    IObservable<bool> Connections { get; }
    //IObservable<ConnectionFailureReason> ConnectionFailures { get; }
    IObservable<Unit> Authentications { get; }

    IObservable<Unit> InvalidUserNames { get; }
    IObservable<Unit> InvalidServiceUrls { get; }

    public void Main()
    {
    }

    public IDisposable Connect() => StableCompositeDisposable.Create(
      users.Connect(),
      serviceUrls.Connect());

    private static Uri TryCreate(string url)
    {
      Uri uri;
      return Uri.TryCreate(url, UriKind.Absolute, out uri)
           ? uri
           : null;
    }

    public sealed class ReactiveInput
    {
      public IObservable<string> UserNames { get; set; }
      public IObservable<string> ServiceUrls { get; set; }
      public IObservable<Unit> Connects { get; set; }
      public IObservable<Unit> Cancellations { get; set; }
      public IObservable<Unit> Authentications { get; set; }
      public IObservable<string> Passwords { get; set; }
    }
  }
}

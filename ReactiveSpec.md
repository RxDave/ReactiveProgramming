# RP
### Business Layer
#### Input
    IObservable<string> UserNames { set; }
    IObservable<Uri> ServiceUrls { set; }
    IObservable<Unit> Connects { set; }
    IObservable<Unit> Cancellations { set; }
    IObservable<Unit> Authentications { set; }
    IObservable<string> Passwords { set; }

#### Output
    IObservable<bool> Connections { get; }
    IObservable<ConnectionFailureReason> ConnectionFailures { get; }
    IObservable<Unit> Authentications { get; }

### Presentation Layer
#### Input
    IObservable<Unit> Authentications { set; }

#### Output
    IObservable<string> Passwords { get; }
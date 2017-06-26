# RP
### Business Layer
#### Input
    IObservable<string> UserNames { set; }
    IObservable<Uri> ServiceUrls { set; }
    IObservable<Unit> Connects { set; }
    IObservable<Unit> Cancellations { set; }

#### Output
    IObservable<bool> Connections { get; }
    IObservable<string> ConnectionFailures { get; }

### Presenting Layer

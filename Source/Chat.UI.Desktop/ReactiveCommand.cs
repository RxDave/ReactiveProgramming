using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace Chat.UI.Desktop
{
  public sealed class ReactiveCommand : ObservableBase<object>, ICommand
  {
    private readonly Subject<object> subject = new Subject<object>();

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
        => true;

    public void Execute(object parameter)
        => subject.OnNext(parameter);

    protected override IDisposable SubscribeCore(IObserver<object> observer)
           => subject.Subscribe(observer);
  }
}

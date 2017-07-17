namespace Chat.UI.Desktop.ViewModels
{
  public sealed class ConnectionViewModel : ViewModel
  {
    private string userName;
    private string serviceUrl;

    public string UserName
    {
      get => userName;
      set => Set(ref userName, value);
    }

    public string ServiceUrl
    {
      get => serviceUrl;
      set => Set(ref serviceUrl, value);
    }

    public ReactiveCommand Connect { get; } = new ReactiveCommand();
  }
}

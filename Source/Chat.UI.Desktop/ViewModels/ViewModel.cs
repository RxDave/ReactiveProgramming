using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.UI.Desktop.ViewModels
{
  public abstract class ViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
    {
      if (!EqualityComparer<T>.Default.Equals(field, value))
      {
        field = value;
        OnPropertyChanged(propertyName);
        return true;
      }

      return false;
    }

    protected void OnAnyPropertyChanged()
           => OnPropertyChanged(new PropertyChangedEventArgs(propertyName: null));

    protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
           => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
           => PropertyChanged?.Invoke(this, e);
  }
}
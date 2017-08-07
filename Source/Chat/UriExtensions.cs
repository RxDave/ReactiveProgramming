using System;

namespace Chat
{
  public static class UriExtensions
  {
    public static Uri TryCreateUri(string url)
    {
      Uri uri;
      return Uri.TryCreate(url, UriKind.Absolute, out uri)
           ? uri
           : null;
    }
  }
}

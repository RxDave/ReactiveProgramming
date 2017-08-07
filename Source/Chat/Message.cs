using System;

namespace Chat
{
  public sealed class Message
  {
    public Message(string text, string fontFamily, double size, Color color)
    {
      Text = text;
      FontFamily = fontFamily;
      Size = size;
      Color = color;
    }

    public Message(User user, DateTimeOffset sent, string text, string fontFamily, double size, Color color)
      : this(text, fontFamily, size, color)
    {
      User = user;
      Sent = sent;
    }

    public User User { get; }

    public DateTimeOffset Sent { get; }

    public string Text { get; }

    public string FontFamily { get; }

    public double Size { get; }

    public Color Color { get; }

    public override string ToString() => Text;
  }
}
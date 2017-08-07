using System;

namespace Chat
{
  public struct Color : IEquatable<Color>
  {
    public static readonly Color Transparent = new Color();

    public Color(byte alpha, byte red, byte green, byte blue)
    {
      Alpha = alpha;
      Red = red;
      Green = green;
      Blue = blue;
    }

    public byte Alpha { get; }

    public byte Red { get; }

    public byte Green { get; }

    public byte Blue { get; }

    public static bool operator ==(Color first, Color second) => first.Equals(second);

    public static bool operator !=(Color first, Color second) => !first.Equals(second);

    public override bool Equals(object obj) => obj is Color && Equals((Color)obj);

    public bool Equals(Color other) => Alpha == other.Alpha
                                    && Red == other.Red
                                    && Green == other.Green
                                    && Blue == other.Blue;

    public override int GetHashCode() => Alpha ^ Red ^ Green ^ Blue;

    public override string ToString() => BitConverter.ToString(new[] { Alpha, Red, Green, Blue });
  }
}

namespace Chat
{
  public sealed class User
  {
    public User(string userName) => UserName = userName;

    public string UserName { get; }

    public static User TryCreate(string userName)
        => userName?.Length == 5
         ? new User(userName)
         : null;
  }
}

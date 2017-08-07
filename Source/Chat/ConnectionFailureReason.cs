namespace Chat
{
  public enum ConnectionFailureReason
  {
    None,
    InvalidUserName,
    InvalidServiceUrl,
    InvalidPassword,
    ServerError,
    Timeout
  }
}
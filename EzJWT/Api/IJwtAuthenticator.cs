namespace EzJWT.Api;

public interface IJwtAuthenticator
{
    public string Key { get; set; }
    public byte[] IV { get; set; }
    public bool IsValidLogin(string username, string password);
}
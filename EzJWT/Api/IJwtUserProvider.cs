namespace EzJWT.Api;

public interface IJwtUserProvider
{
    public IJwtUser GetUser(string username);
}
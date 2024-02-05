using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzJWT.Api;

[ApiController]
[Route("auth")]
public class JwtAuthenticationController : ControllerBase
{
    private IJwtAuthenticator authenticator;
    private IJwtUserProvider provider;

    public JwtAuthenticationController(IJwtAuthenticator _authenticator, IJwtUserProvider _provider)
    {
        authenticator = _authenticator;
        provider = _provider;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get([FromHeader]string username, [FromHeader]string plaintext)
    {
        if (authenticator.IsValidLogin(username, plaintext))
        {
            using (Jwt jwt = new Jwt(authenticator.Key, authenticator.IV))
            {
                IJwtUser user = provider.GetUser(username) ?? throw new NullReferenceException("User cant be null!");
                
                ClaimCollection ret = new();
                ret.AddClaim("userid", user.UserId);
                ret.AddExpiration(DateTime.UtcNow.AddMonths(1));
                
                string token = ret.CreateJwt(jwt);

                return Ok(new { token = token });
            }
        } else {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }

    [HttpGet]
    [Route("isauthed")]
    public async Task<ActionResult<object>> IsAuthed([FromHeader]string token)
    {
        using (Jwt jwt = new Jwt(authenticator.Key, authenticator.IV))
        {
            try
            {
                ClaimCollection claims = ClaimCollection.FromJwt(jwt, token);

                if(!claims.IsExpired)
                {
                    return Ok(new { userid = claims["userid"]!.ToString() });
                } else {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Session expired!" });
                }
            } catch (NullReferenceException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Not Authed!" });
            }
        }
    }
}
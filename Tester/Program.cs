using System.Security.Claims;
using System.Security.Cryptography;
using EzJWT;

const string key = "my16characterkey";
byte[] iv = RandomNumberGenerator.GetBytes(16);

string token = "";

using (Jwt jwt = new Jwt(key, iv))
{
    ClaimCollection collection = new ClaimCollection();
    collection.AddClaim("userId", Guid.NewGuid().ToString());
    collection.AddClaim("userName", "JohnDoe");
    collection.AddExpiration(DateTimeOffset.UtcNow.AddSeconds(1));

    token = collection.CreateJwt(jwt);
}

using (Jwt jwt = new Jwt(key, iv))
{
    ClaimCollection collection = ClaimCollection.FromJwt(jwt, token);

    foreach (KeyValuePair<string, object> val in collection)
    {
        Console.WriteLine(val.Key + " : " + val.Value);
    }
}
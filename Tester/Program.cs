using System.Security.Claims;
using EzJWT;

const string key = "my16characterkey";

using (Jwt jwt = new Jwt(key))
{
    ClaimCollection collection = new ClaimCollection();
    collection.AddClaim("userId", Guid.NewGuid().ToString());
    collection.AddClaim("userName", "JohnDoe");
    collection.AddExpiration(DateTimeOffset.UtcNow.AddSeconds(1));

    Thread.Sleep(5000);
    
    string serialized = collection.CreateJwt(jwt);
    ClaimCollection deserialized = ClaimCollection.FromJwt(jwt, serialized) ?? throw new NullReferenceException();

    Console.WriteLine("Serialized: " + serialized);

    Console.WriteLine("Deserialized:");
    foreach (KeyValuePair<string, object?> element in deserialized)
    {
        Console.WriteLine(element.Key + " : " + element.Value);
    }

    Console.WriteLine(deserialized.IsExpired);
    
}
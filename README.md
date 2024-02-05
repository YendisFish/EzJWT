# EzJWT

Yes, this is not actually a JWT library! However, it improves upon the JWT
standard in a few ways. This library ensures one source of truth, makes creating
web tokens a very easy process, and removes redundant tasks from the JWT process!

# Requirements

In order to use and build this library you will need to have dotnet 8! Preferably
you will also need an API.

# How To Use

At its most basic level EzJWT provides 2 classes to help you create a token. Here
is an example:

Make sure you have your key:

```csharp
const string Key = "my16characterkey";
```

Create a Jwt instance, make your claims object, and create your token:
```csharp
using(Jwt jwt = new Jwt(Key))
{
    ClaimsCollection claims = new ClaimsCollection();
    claims.AddClaim("UserId", "someuserid");
    
    //By default the token never expires, you can add expirations though!
    claims.AddExpiration(DateTimeOffset.UtcNow.AddDays(7));
    
    string token = claims.CreateJwt(jwt);
}
```

You can also, however, create a JWT from whatever object you would like!

```csharp
class MyClass
{
    string id { get; set; } = Guid.NewGuid().ToString();
    int foo { get; set; } = 5;
}
```
```csharp
using(Jwt jwt = new Jwt(Key))
{
    string token = jwt.Serialize(new MyClass());
}
```

### Checking Claim Expiration
The ClaimCollection object contains a variable that will determine whether
or not it has expired:

```csharp
ClaimCollection incomingClaims = ClaimCollection.FromJwt(jwt, token);

if(!incomingClaims.IsExpired)
{
    //do something
}
```

# Todo
- [ ] Add extension methods to objects
- [ ] Extend encryption options for Jwt class
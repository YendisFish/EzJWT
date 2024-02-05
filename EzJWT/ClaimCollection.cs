namespace EzJWT;

public class ClaimCollection
{
    private Dictionary<string, object?> raw { get; init; }
    public bool IsExpired => (raw.ContainsKey("expiration")) ? (long)raw["expiration"]! <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() : false;

    public KeyValuePair<string, object?> this[int index]
    {
        get => raw.ElementAt(index);
    }
    
    public object? this[string index]
    {
        get => raw[index];
        set => raw[index] = value;
    }

    public ClaimCollection()
    {
        raw = new();
    }
    
    internal ClaimCollection(Dictionary<string, object?> incoming)
    {
        raw = incoming;
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => raw.GetEnumerator();
    
    public void AddClaim(string key, object? value)
    {
        raw.Add(key, value);
    }

    public void AddExpiration(long offset)
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long expiration = currentTime + offset;
        
        raw.Add("expiration", expiration);
    }
    
    public void AddExpiration(DateTimeOffset expiration) => raw.Add("expiration", expiration.ToUnixTimeMilliseconds());

    public object? GetClaim(string key) => raw[key];
    public void RemoveClaim(string key) => raw.Remove(key);
    public void ClearClaims() => raw.Clear();

    public string CreateJwt(Jwt jwt) => jwt.Serialize(this.raw);
    public static ClaimCollection FromJwt(Jwt jwt, string token) => new(jwt.Deserialize<Dictionary<string, object?>>(token) ?? throw new NullReferenceException());
}
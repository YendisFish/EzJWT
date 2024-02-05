using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace EzJWT;

public class Jwt : IDisposable
{
    public string Key { get; init; }
    private SymmetricAlgorithm aes { get; set; } // eventually maybe I will support more than AES

    public Jwt(string? key = null)
    {
        Key = string.IsNullOrEmpty(key) ? Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)) : key;

        aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
    }

    public string Serialize<T>(T data) => this.Encrypt(JsonConvert.SerializeObject(data));
    public T? Deserialize<T>(string data) => JsonConvert.DeserializeObject<T?>(this.Decrypt(data));
    
    #region AES Functions

    internal string Encrypt(string raw)
    {
        ICryptoTransform encryptor = aes.CreateEncryptor();

        using (MemoryStream ms = new MemoryStream())
        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(raw);
            }
            
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    internal string Decrypt(string encrypted)
    {
        ICryptoTransform decryptor = aes.CreateDecryptor();

        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encrypted)))
        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (StreamReader sr = new StreamReader(cs))
        {
            return sr.ReadToEnd();
        }
    }
    
    #endregion

    public void Dispose()
    {
        aes.Dispose();
    }
}
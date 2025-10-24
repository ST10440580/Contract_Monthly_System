using Microsoft.AspNetCore.Http;
using System.Text;

public class MockHttpSession : ISession
{
    private Dictionary<string, byte[]> _sessionStorage = new();

    public IEnumerable<string> Keys => _sessionStorage.Keys;
    public string Id => Guid.NewGuid().ToString();
    public bool IsAvailable => true;

    public void Clear() => _sessionStorage.Clear();
    public void Remove(string key) => _sessionStorage.Remove(key);
    public void Set(string key, byte[] value) => _sessionStorage[key] = value;

    public bool TryGetValue(string key, out byte[] value)
    {
        return _sessionStorage.TryGetValue(key, out value);
    }

    public void SetInt32(string key, int value) => Set(key, BitConverter.GetBytes(value));
    public int? GetInt32(string key)
    {
        return _sessionStorage.TryGetValue(key, out var value) ? BitConverter.ToInt32(value, 0) : null;
    }

    public void SetString(string key, string value) => Set(key, Encoding.UTF8.GetBytes(value));
    public string GetString(string key)
    {
        return _sessionStorage.TryGetValue(key, out var value) ? Encoding.UTF8.GetString(value) : null;
    }
}

using System.Text;
using UnitTesting_Contract_Monthly_System.Tests.Services;

public class MockHttpSession : ISession
{
    private Dictionary<string, object> _sessionStorage = new();

    public IEnumerable<string> Keys => _sessionStorage.Keys;

    public string Id => Guid.NewGuid().ToString();

    public bool IsAvailable => true;

    public void Clear() => _sessionStorage.Clear();

    public void Remove(string key) => _sessionStorage.Remove(key);

    public void Set(string key, byte[] value) => _sessionStorage[key] = Encoding.UTF8.GetString(value);

    public bool TryGetValue(string key, out byte[] value)
    {
        if (_sessionStorage.TryGetValue(key, out var obj))
        {
            value = Encoding.UTF8.GetBytes(obj.ToString());
            return true;
        }

        value = null;
        return false;
    }

    public void SetInt32(string key, int value) => _sessionStorage[key] = value.ToString();

    public int? GetInt32(string key)
    {
        if (_sessionStorage.TryGetValue(key, out var obj) && int.TryParse(obj.ToString(), out var result))
        {
            return result;
        }
        return null;
    }

    public void SetString(string key, string value) => _sessionStorage[key] = value;

    public string GetString(string key) => _sessionStorage.TryGetValue(key, out var obj) ? obj.ToString() : null;
}

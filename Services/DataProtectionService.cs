using Microsoft.AspNetCore.DataProtection;

namespace EventFlow.Services;

public class DataProtectionService
{
    private readonly IDataProtector _protector;

    public DataProtectionService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("EventFlow.Security");
    }

    public string Encrypt(string value)
    {
        return _protector.Protect(value);
    }

    public string Decrypt(string value)
    {
        return _protector.Unprotect(value);
    }
}
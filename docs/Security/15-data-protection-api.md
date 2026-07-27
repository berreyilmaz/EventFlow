# Lab 15 – Data Protection API

## Objective

The purpose of this lab is to protect sensitive application data using the ASP.NET Core Data Protection API.

Instead of storing sensitive information in plain text, the application encrypts the data before storage and decrypts it only when needed.

---

## Vulnerability

Storing sensitive information in plain text can expose confidential data if the database or application storage is compromised.

Examples include:

- API Keys
- Secret Tokens
- External Service Credentials
- Sensitive Configuration Values

Anyone with database access could read these values directly.

---

## Solution

ASP.NET Core Data Protection API was implemented to encrypt and decrypt sensitive data.

A custom service named `DataProtectionService` was created to centralize encryption operations.

The service:

- Encrypts sensitive values before storage.
- Decrypts protected values when required.
- Uses the built-in ASP.NET Core cryptographic infrastructure.

---

## Configuration

Data Protection services were registered in the dependency injection container.

```csharp
builder.Services.AddDataProtection();

builder.Services.AddScoped<DataProtectionService>();
```

---

## Data Protection Service

```csharp
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
```

---

## Demonstration

A sample controller was created to demonstrate the encryption process.

The workflow is:

1. Original text is created.
2. The text is encrypted.
3. The encrypted value is displayed.
4. The encrypted value is decrypted.
5. The original value is successfully restored.

---

## Example

Original Value

```text
EventFlow-Secret-Token
```

Encrypted Value

```text
CfDJ8...
```

Decrypted Value

```text
EventFlow-Secret-Token
```

The encrypted value is unreadable and can only be restored using the application's Data Protection keys.

---

## Testing

The implementation was tested by:

- Encrypting a sample string.
- Verifying that the encrypted output differs from the original value.
- Decrypting the encrypted value.
- Confirming that the decrypted result matches the original text.

---

## Security Benefits

- Prevents sensitive data from being stored in plain text.
- Uses Microsoft's built-in cryptographic implementation.
- Simplifies encryption and decryption.
- Protects application secrets from unauthorized disclosure.
- Follows ASP.NET Core security best practices.

---

## Result

Sensitive application data is now protected using the ASP.NET Core Data Protection API.

Only the application can decrypt protected values, significantly improving the confidentiality of sensitive information stored by EventFlow.
# Lab 14 – Strong Password Policy

## Objective

The purpose of this lab is to enforce a strong password policy using ASP.NET Core Identity. Strong password requirements help protect user accounts against brute force, dictionary, and credential stuffing attacks.

---

## Vulnerability

Weak passwords significantly increase the risk of unauthorized account access.

Common issues include:

- Short passwords
- Passwords containing only letters or numbers
- Easily guessable passwords
- Reused passwords

Attackers can exploit weak passwords using automated password guessing techniques.

---

## Solution

ASP.NET Core Identity password requirements were configured to enforce strong password complexity rules.

The password policy requires:

- Minimum password length
- At least one uppercase letter
- At least one lowercase letter
- At least one numeric digit
- At least one special character
- Minimum number of unique characters

---

## Configuration

```csharp
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredUniqueChars = 3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

---

## Password Requirements

| Requirement | Value |
|-------------|-------|
| Minimum Length | 8 characters |
| Uppercase Letter | Required |
| Lowercase Letter | Required |
| Numeric Digit | Required |
| Special Character | Required |
| Unique Characters | Minimum 3 |

---

## Testing

### Invalid Password Examples

The following passwords should be rejected:

```text
12345678
```

```text
password
```

```text
Password
```

```text
Password1
```

---

### Valid Password Examples

The following passwords satisfy the configured policy:

```text
Password1!
```

```text
EventFlow2026!
```

---

## Validation

ASP.NET Core Identity automatically validates the password during user registration.

If the password does not meet the configured requirements, descriptive validation messages are displayed to the user.

Example validation messages include:

- Passwords must have at least one uppercase letter.
- Passwords must have at least one lowercase letter.
- Passwords must have at least one digit.
- Passwords must have at least one non alphanumeric character.

---

## Security Benefits

- Reduces the risk of brute force attacks.
- Prevents the use of weak passwords.
- Increases password complexity.
- Improves overall account security.
- Follows ASP.NET Core Identity security best practices.

---

## Result

A strong password policy has been successfully implemented using ASP.NET Core Identity.

Users are now required to create complex passwords that satisfy defined security requirements, significantly improving authentication security and reducing the likelihood of compromised user accounts.
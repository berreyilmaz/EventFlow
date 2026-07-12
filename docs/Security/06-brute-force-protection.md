# Brute Force Protection Laboratory

## Objective

Protect user accounts against repeated failed login attempts.

---

## Vulnerability

Without account lockout, an attacker can repeatedly try passwords until the correct one is found.

Example attack:

```
admin@test.com

123456
password
admin123
qwerty
...
```

---

## Security Controls

ASP.NET Identity Lockout was enabled.

Configuration:

```csharp
options.Lockout.MaxFailedAccessAttempts = 5;

options.Lockout.DefaultLockoutTimeSpan =
    TimeSpan.FromMinutes(5);

options.Lockout.AllowedForNewUsers = true;
```

---

## Login Configuration

```csharp
lockoutOnFailure: true
```

---

## Test

Wrong password entered five times.

Result:

```
Account locked for 5 minutes.
```

Correct password during lockout:

```
Login denied.
```

---

## Result

The application is protected against brute force login attempts using ASP.NET Identity account lockout.
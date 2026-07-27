# Lab 13 – Secure Authentication Cookies

## Objective

The purpose of this lab is to configure secure authentication cookies in ASP.NET Core Identity to protect user sessions against common web attacks such as Cross-Site Scripting (XSS) and Cross-Site Request Forgery (CSRF).

---

## Vulnerability

Authentication cookies store the user's authenticated session.

If cookies are not configured securely:

- JavaScript may steal session cookies through XSS attacks.
- Cookies may be transmitted over unencrypted HTTP connections.
- Cross-site requests may automatically include authentication cookies.
- Long-lived sessions increase the risk of session hijacking.

Improper cookie configuration can expose sensitive user sessions to attackers.

---

## Solution

The ASP.NET Core Identity authentication cookie was configured with secure settings using `ConfigureApplicationCookie()`.

The following security measures were applied:

- Custom cookie name
- HttpOnly enabled
- Secure cookies (HTTPS only)
- SameSite protection
- Session expiration
- Sliding expiration

---

## Configuration

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "EventFlow.Auth";

    options.Cookie.HttpOnly = true;

    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Strict;

    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.SlidingExpiration = true;
});
```

---

## Security Features

### HttpOnly

The authentication cookie cannot be accessed by JavaScript.

This helps mitigate cookie theft through Cross-Site Scripting (XSS) attacks.

---

### Secure Policy

The authentication cookie is transmitted only over HTTPS connections.

This prevents cookies from being exposed over unencrypted HTTP traffic.

---

### SameSite

`SameSite = Strict` ensures that the authentication cookie is only sent for requests originating from the same site.

This significantly reduces the risk of Cross-Site Request Forgery (CSRF) attacks.

---

### Expiration

Authentication cookies automatically expire after **30 minutes** of inactivity.

This reduces the lifetime of compromised sessions.

---

### Sliding Expiration

If the user remains active, the session expiration time is automatically extended.

This improves usability while maintaining session security.

---

## Verification

The cookie configuration was verified using the browser's Developer Tools.

The authentication cookie contained the following properties:

| Property | Value |
|----------|-------|
| Name | EventFlow.Auth |
| HttpOnly | Enabled |
| Secure | Enabled |
| SameSite | Strict |
| Expiration | Session / Configured Lifetime |

---

## Security Benefits

- Protects authentication cookies from JavaScript access.
- Prevents cookies from being sent over unsecured HTTP connections.
- Reduces CSRF attack risks.
- Improves session security.
- Limits session lifetime.
- Uses ASP.NET Core Identity best practices.

---

## Result

Secure authentication cookie settings have been successfully implemented in EventFlow.

Authentication cookies are now protected using HttpOnly, Secure, SameSite, expiration, and sliding expiration settings, providing a more secure session management mechanism for authenticated users.
# HTTP Security Headers

## Purpose

Improve the application's security by adding HTTP response headers that help protect against common web-based attacks.

## Risk

Without security headers, an application may be vulnerable to:

- Clickjacking
- MIME Type Sniffing
- Information Disclosure
- Unauthorized Browser Features

## Vulnerable Scenario

The application returns HTTP responses without any additional security headers.

As a result, browsers may perform unsafe behaviors such as MIME sniffing or allowing the application to be embedded inside an iframe.

## Secure Implementation

A custom middleware was added to include security headers in every HTTP response.

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] =
        "camera=(), microphone=(), geolocation=()";

    await next();
});
```

## Security Headers

| Header | Purpose |
|----------|---------|
| X-Content-Type-Options | Prevents MIME type sniffing |
| X-Frame-Options | Protects against Clickjacking attacks |
| Referrer-Policy | Limits the information sent in the Referer header |
| Permissions-Policy | Restricts access to browser features such as camera, microphone, and geolocation |

## Testing

The response headers were verified using the browser's Developer Tools (Network tab).

Expected headers:

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: camera=(), microphone=(), geolocation=()
```

## Result

The application now returns essential HTTP security headers, reducing exposure to several common web security risks.
# Content Security Policy (CSP)

## Purpose

Protect the application against Cross-Site Scripting (XSS) attacks by restricting the sources from which browsers are allowed to load resources.

## Risk

Without a Content Security Policy, attackers may inject malicious JavaScript into web pages through vulnerabilities such as Cross-Site Scripting (XSS).

If executed, malicious scripts can:

- Steal session cookies
- Access sensitive user data
- Perform actions on behalf of authenticated users
- Redirect users to malicious websites

## Vulnerable Scenario

The application allows browsers to execute scripts without restricting their origin.

An attacker who successfully injects JavaScript may execute arbitrary code in the victim's browser.

## Secure Implementation

A Content Security Policy header was added using custom middleware.

```csharp
context.Response.Headers["Content-Security-Policy"] =
    "default-src 'self'; " +
    "script-src 'self'; " +
    "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
    "font-src 'self' https://cdn.jsdelivr.net; " +
    "img-src 'self' data:;";
```

## Policy Description

| Directive | Purpose |
|------------|---------|
| default-src 'self' | Allows resources only from the same origin by default |
| script-src 'self' | Allows JavaScript only from the application's own origin |
| style-src | Allows local styles and trusted CDN styles |
| font-src | Allows fonts only from trusted sources |
| img-src | Allows local images and Base64 encoded images |

## Testing

The Content-Security-Policy header was verified using the browser Developer Tools.

The application loaded successfully while restricting resource origins according to the configured policy.

## Result

The application now enforces a Content Security Policy, providing an additional layer of defense against Cross-Site Scripting (XSS) attacks.
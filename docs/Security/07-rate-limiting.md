# Rate Limiting

## Purpose

Prevent users or attackers from sending an excessive number of HTTP requests within a short period of time.

## Risk

Without rate limiting, an application is vulnerable to:

- Brute Force attacks
- Denial of Service (DoS)
- API Abuse
- Resource Exhaustion

These attacks can degrade application performance or make the service unavailable.

## Vulnerable Scenario

The application accepts an unlimited number of requests without any restrictions.

## Secure Implementation

ASP.NET Core Rate Limiter Middleware was configured using a fixed window policy.

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
});
```

The Login endpoints were protected using:

```csharp
[EnableRateLimiting("fixed")]
```

## Testing

- Limit: **10 requests per minute**
- Test: Send more than 10 requests to the Login page within one minute.
- Expected Result:

```
HTTP 429 Too Many Requests
```

The application returns a custom message informing the user that too many requests have been sent.

## Result

The application is protected against excessive requests, reducing the risk of brute force attacks and request flooding.
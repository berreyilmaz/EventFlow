# Lab 12 – Global Exception Logging

## Objective

The purpose of this lab is to implement a centralized exception logging mechanism that automatically captures unhandled exceptions across the application. This helps administrators diagnose unexpected errors, monitor application stability, and investigate production issues.

---

## Vulnerability

Without centralized exception logging:

- Application crashes may go unnoticed.
- Important debugging information can be lost.
- Developers have limited visibility into runtime errors.
- Administrators cannot easily investigate failures.

Attackers may also intentionally trigger exceptions to discover weaknesses if errors are not properly handled.

---

## Solution

A custom middleware named `ExceptionLoggingMiddleware` was implemented.

The middleware:

- Catches all unhandled exceptions.
- Stores exception details in the database.
- Records the authenticated user (or `Anonymous`).
- Records the request path.
- Records the client IP address.
- Saves the exception type and message.
- Redirects the user to the application's error page.

This ensures that every unexpected exception is logged consistently without exposing sensitive information to end users.

---

## Database Model

```csharp
public class ExceptionLog
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string ExceptionType { get; set; }

    public string Message { get; set; }

    public string Path { get; set; }

    public string IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}
```

---

## Middleware

The custom middleware wraps every incoming request inside a `try-catch` block.

If an exception occurs:

1. The exception information is collected.
2. A new `ExceptionLog` record is created.
3. The log is stored in the database.
4. The user is redirected to the error page.

---

## Logged Information

Each exception log contains:

| Field | Description |
|--------|-------------|
| UserName | Authenticated user or Anonymous |
| ExceptionType | Exception class name |
| Message | Exception message |
| Path | Requested URL |
| IpAddress | Client IP address |
| CreatedAt | UTC timestamp |

---

## Admin Monitoring

An **Exception Logs** page was created for administrators.

The page displays:

- Exception type
- Error message
- User
- Request path
- Client IP
- Timestamp

Only administrators are allowed to access this page.

---

## Testing

A test endpoint was created that intentionally throws an exception.

```csharp
throw new Exception("This is a test exception.");
```

After triggering the endpoint:

- The exception was automatically logged.
- A database record was created.
- The user was redirected to the error page.
- The administrator could review the exception from the Exception Logs page.

---

## Security Benefits

- Centralized exception handling
- Improved application monitoring
- Faster troubleshooting
- Better production diagnostics
- Prevents loss of critical error information
- Supports incident investigation

---

## Result

Global exception logging has been successfully implemented using custom middleware.

Unexpected runtime errors are now automatically captured, stored securely in the database, and made available to administrators through a dedicated monitoring interface, improving both application reliability and security visibility.
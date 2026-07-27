# Lab 10 – Audit Logging

## Objective

The objective of this lab is to implement an audit logging mechanism that records important user activities within the EventFlow application. Audit logs provide visibility into user actions, improve accountability, and support security monitoring and incident investigation.

---

## Security Risk

Without audit logging:

- User activities cannot be traced.
- Security incidents are difficult to investigate.
- Malicious or unauthorized actions may go unnoticed.
- Administrators have no historical record of critical operations.

Maintaining an audit trail is an important security practice for monitoring system activity and supporting compliance requirements.

---

## Implementation

A dedicated **AuditLog** entity was created to store audit records in the database.

### Stored Information

Each audit record contains:

- Username
- Action
- Description
- IP Address
- Timestamp

### AuditLog Model

```csharp
public class AuditLog
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

---

## Audit Service

To centralize audit logging, an `IAuditService` interface and an `AuditService` implementation were created.

### Interface

```csharp
public interface IAuditService
{
    Task LogAsync(
        string action,
        string description,
        HttpContext httpContext);
}
```

### Service Implementation

```csharp
public async Task LogAsync(
    string action,
    string description,
    HttpContext httpContext)
{
    var log = new AuditLog
    {
        UserName = httpContext.User.Identity?.Name ?? "Anonymous",
        Action = action,
        Description = description,
        IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
        CreatedAt = DateTime.UtcNow
    };

    _context.AuditLogs.Add(log);

    await _context.SaveChangesAsync();
}
```

---

## Logged Operations

Audit logs are automatically created after the following actions:

- Create Event
- Update Event
- Delete Event
- Join Event
- Leave Event

Example:

```csharp
await _auditService.LogAsync(
    "Create Event",
    $"Created event '{eventEntity.Title}'",
    HttpContext);
```

---

## Dependency Injection

The audit service was registered using the built-in dependency injection container.

```csharp
builder.Services.AddScoped<IAuditService, AuditService>();
```

---

## Audit Log Administration

A dedicated **Audit Logs** page was implemented for administrators.

Only users with the **Admin** role can access this page.

```csharp
[Authorize(Roles = "Admin")]
public class AuditController : Controller
{
    ...
}
```

The page displays:

- Date and Time
- Username
- Action
- Description
- IP Address

This enables administrators to review application activity from a single interface.

---

## Testing

The following scenarios were tested successfully:

- Creating a new event
- Updating an existing event
- Deleting an event
- Joining an event
- Leaving an event

Each action generated a corresponding record in the **AuditLogs** table.

Example audit records:

| User | Action | Description |
|------|--------|-------------|
| admin@example.com | Create Event | Created event 'ASP.NET Workshop' |
| admin@example.com | Update Event | Updated event 'ASP.NET Workshop' |
| user@example.com | Join Event | Joined event 'ASP.NET Workshop' |
| user@example.com | Leave Event | Left event 'ASP.NET Workshop' |

---

## Security Benefits

This implementation provides:

- Complete audit trail
- User accountability
- Activity monitoring
- Administrative visibility
- Support for incident investigation
- Improved security monitoring

Audit logging enhances the overall security of the EventFlow application by recording important user activities and providing administrators with valuable operational insight.

---

## Result

A centralized audit logging mechanism has been successfully integrated into the EventFlow application.

Critical user actions are automatically recorded, stored securely in the database, and made available through an administrator-only interface for monitoring and investigation.
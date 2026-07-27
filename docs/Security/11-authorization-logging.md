# Lab 11 – Authorization Logging

## Objective

The objective of this lab is to record unauthorized access attempts within the EventFlow application.

While standard audit logging records successful user actions, authorization logging captures failed authorization attempts, providing administrators with visibility into suspicious or malicious activities.

---

## Security Risk

Without authorization logging:

- Unauthorized access attempts remain unnoticed.
- Administrators cannot identify users attempting to access restricted resources.
- Suspicious behavior cannot be investigated effectively.
- Security monitoring becomes significantly more difficult.

Recording authorization failures improves accountability and strengthens the application's overall security monitoring capabilities.

---

## Implementation

Authorization checks were enhanced to log unauthorized requests before denying access.

Instead of immediately returning a **403 Forbidden** response, the application first records the attempt in the audit log.

Example:

```csharp
if (!User.IsInRole("Admin") &&
    eventEntity.OrganizerId != currentUserId)
{
    await _auditService.LogAsync(
        "Unauthorized Access",
        $"Attempted to edit event '{eventEntity.Title}'",
        HttpContext);

    return Forbid();
}
```

---

## Protected Operations

Authorization logging was implemented for the following operations:

- Edit Event
- Update Event
- Delete Event
- Accessing the Audit Logs page

Each unauthorized request is recorded before the request is rejected.

---

## Audit Log Example

Example audit record:

| User | Action | Description |
|------|--------|-------------|
| organizer@example.com | Unauthorized Access | Attempted to edit event 'Hackathon' |
| user@example.com | Unauthorized Access | Attempted to delete event 'Conference' |
| organizer@example.com | Unauthorized Access | Attempted to access Audit Logs |

---

## Audit Controller Protection

The Audit Logs page is available only to administrators.

Before returning the audit log list, the application verifies the user's role.

```csharp
if (!User.IsInRole("Admin"))
{
    await _auditService.LogAsync(
        "Unauthorized Access",
        "Attempted to access Audit Logs",
        HttpContext);

    return Forbid();
}
```

This ensures that unauthorized attempts to access sensitive administrative information are also recorded.

---

## Testing

The following scenarios were tested:

- Organizer attempting to edit another user's event
- Organizer attempting to delete another user's event
- Non-admin user attempting to access the Audit Logs page

Expected result:

- HTTP 403 Forbidden is returned.
- A new audit log entry is created.
- The administrator can later review the attempt from the Audit Logs panel.

---

## Security Benefits

This implementation provides:

- Detection of unauthorized access attempts
- Improved security monitoring
- Better incident investigation
- Increased accountability
- Visibility into suspicious user behavior

Authorization logging complements traditional audit logging by recording both successful operations and failed authorization attempts.

---

## Result

Unauthorized access attempts are now recorded before access is denied.

Administrators can review these events through the Audit Logs page, providing a complete history of both successful user actions and blocked authorization attempts.

This implementation significantly improves the application's monitoring and incident response capabilities.
# CSRF (Cross-Site Request Forgery)

## Vulnerability

The POST Delete endpoint accepted requests without verifying an anti-forgery token.

## Attack

A malicious HTML form submitted a POST request to:

/Event/Delete

The authenticated user's browser automatically sent the authentication cookie.

As a result, the event was deleted without the user's consent.

## Fix

Added:

[ValidateAntiForgeryToken]

All Razor forms automatically include the anti-forgery token.

Requests without a valid token are rejected.
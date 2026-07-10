# IDOR (Insecure Direct Object Reference)

## Description

An authenticated organizer could edit another user's event by changing the event ID in the URL.

Example:

/Event/Edit/1

↓

/Event/Edit/2

## Risk

Broken Access Control

## Fix

The application now verifies that the current user is either:

- The owner of the event (`OrganizerId`)
- An administrator (`Admin` role)

Otherwise, it returns **403 Forbidden**.
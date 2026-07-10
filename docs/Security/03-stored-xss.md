# Stored XSS Laboratory

## Objective

Demonstrate how rendering user input without HTML encoding can lead to a Stored Cross-Site Scripting (XSS) vulnerability.

---

## Attack

An event was created with the following description:

```html
<script>alert("XSS TEST")</script>
```

When rendered using:

```cshtml
@Html.Raw(Model.Description)
```

the browser executed the JavaScript code.

---

## Root Cause

Using `@Html.Raw()` disables Razor's automatic HTML encoding.

As a result, user-controlled HTML is rendered directly into the page.

---

## Fix

Render user input normally:

```cshtml
@Model.Description
```

Razor automatically HTML-encodes user input and prevents script execution.

---

## Result

✔ Stored XSS vulnerability reproduced.

✔ Vulnerability fixed by removing `Html.Raw`.
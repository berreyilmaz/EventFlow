# File Upload Security Laboratory

## Objective

Implement secure image upload functionality for events while preventing
common file upload vulnerabilities.

---

## Vulnerability

Allowing unrestricted file uploads may lead to:

- Uploading executable files (.exe, .dll)
- Uploading server-side scripts (.php)
- Malware distribution
- Denial of Service using very large files
- File type spoofing

---

## Security Controls Implemented

### 1. Allowed File Extensions

Only the following extensions are accepted:

- .jpg
- .jpeg
- .png

```csharp
var allowedExtensions = new[]
{
    ".jpg",
    ".jpeg",
    ".png"
};
```

---

### 2. File Size Validation

Maximum upload size:

```
2 MB
```

```csharp
if (model.Image.Length > 2 * 1024 * 1024)
```

---

### 3. MIME Type Validation

Accepted MIME types:

- image/jpeg
- image/png

```csharp
var allowedContentTypes = new[]
{
    "image/jpeg",
    "image/png"
};
```

---

### 4. Magic Number Validation

The application validates the file signature
instead of trusting the file extension.

Supported signatures:

JPEG

```
FF D8 FF
```

PNG

```
89 50 4E 47
```

---

### 5. Random File Name

Uploaded files are stored using a GUID.

Example:

```
6d2dfc74-7d33-4c64-a98d-b9bca7d3b9d1.png
```

This prevents:

- File overwrite
- Predictable file names

---

### 6. Upload Directory

```
wwwroot/uploads/events
```

---

## Attack Scenarios Tested

### EXE Upload

```
virus.exe
```

Result

Rejected

---

### PDF Upload

```
report.pdf
```

Result

Rejected

---

### Large Image (>2MB)

Result

Rejected

---

### Fake Image Extension

```
virus.jpg
```

(real executable renamed)

Result

Rejected by Magic Number validation.

---

## Result

The application now protects against common file upload attacks by using:

- Extension validation
- File size validation
- MIME type validation
- Magic Number validation
- Randomized file names
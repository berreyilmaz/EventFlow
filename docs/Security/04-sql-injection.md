# SQL Injection Laboratory

## Vulnerability

The application concatenated user input directly into the SQL query.

```csharp
command.CommandText =
    $"SELECT * FROM Events WHERE Title = '{search}'";
```

## Attack

Payload:

```text
' OR 1=1 --
```

Generated SQL:

```sql
SELECT * FROM Events
WHERE Title = '' OR 1=1 --'
```

Result:

All event records were returned.

## Fix

Parameterized query:

```csharp
command.CommandText =
    "SELECT * FROM Events WHERE Title = @title";

var parameter = command.CreateParameter();
parameter.ParameterName = "@title";
parameter.Value = search;

command.Parameters.Add(parameter);
```

## Result

The same payload no longer modified the SQL query.
SQL Injection was successfully prevented.
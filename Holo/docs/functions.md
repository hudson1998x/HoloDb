# Stored Functions

Stored functions allow you to define reusable query procedures that can be called from within other functions or queries.

## Syntax

```
function FunctionName($param1: Type, $param2: Type): ReturnType {
    // statements
}
```

## Rules

### Function Definition
- Functions must be defined at the **top level** of a query document
- Functions must specify a **return type** (no inference)
- Function names follow standard identifier rules

### Parameters
- All parameters must be prefixed with `$` (e.g., `$userId`)
- All parameters must have an **explicit type** (error if missing)
- Parameters are separated by commas
- Empty parameter lists `()` are allowed

### Return Value
- The **last NamedBlockNode** in the function body is assumed to be the return value
- Using the `return` keyword throws a syntax error:
  > "Bottom named blocks are assumed as the return value - do not use 'return'."

### Body
- Body contains zero or more statements:
  - Variable declarations (`$var: Type = expression`)
  - Function calls (built-in)
  - Function invocations (user-defined)
  - NamedBlockNodes (output)

## Examples

### Simple Function (No Parameters)

```
function getActiveUser(): User {
    User { id, name, email }
    where {
        active { eq true }
    }
}
```

### Function With Parameters

```
function getUserProjects($userId: int): User {
    User { id, name, projects { id, title } }
    where {
        id { eq $userId }
    }
}
```

### Function With Multiple Parameters

```
function getUsers($limit: int, $offset: int): User {
    User { id, name }
    limit $limit
    skip $offset
}
```

### Function With Variable Declarations

```
function getUserWithRole($userId: int): User {
    $role: string = getUserRole($userId)
    
    User { id, name, role }
    where {
        id { eq $userId }
    }
}
```

### Calling User-Defined Functions

Functions can call other user-defined functions:

```
function getAdminProjects($adminId: int): Project {
    $role: string = getUserRole($adminId)
    
    Project { id, title, owner }
    where {
        owner { eq $adminId }
        role { eq $role }
    }
}
```

## Function Storage

Functions are stored at runtime in the `FunctionRegistry` (a thread-safe `ConcurrentDictionary`). When a query document is parsed:

1. Function definitions are extracted and registered
2. The query portion executes with access to registered functions
3. Function calls in the query body are resolved against the registry

## Error Messages

| Error | Meaning |
|-------|---------|
| `Expected '$' for parameter prefix` | Parameter missing `$` prefix |
| `Bottom named blocks are assumed as the return value - do not use 'return'.` | Used `return` keyword |
| `Unexpected token` | Syntax error in function definition |

## Implementation Details

- Functions are parsed by `FunctionDefinitionProduction`
- Stored as `StoredFunction` objects containing:
  - `Name`: Function identifier
  - `Parameters`: Array of parameter names
  - `ParameterTypes`: Dictionary mapping parameter names to .NET types
  - `BodyAST`: The function's syntax tree
  - `ReturnType`: The .NET type for the return value

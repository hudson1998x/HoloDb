# Dynamic Types in Holo

This document describes how dynamic schema types work in the Holo runtime, how to define them, what limitations exist, and how errors typically arise during parsing, generation, and usage.

## Core concepts
- TypeDefinition: a named schema block declared with the keyword type, e.g. `type User { ... }`.
- FieldDefinition: a field within a type, e.g. `id { type(int) primary() ... }`.
- FieldProperty: a property on a field (type, default, primary, nullable, unique, sensitive, comment, validate, etc.).
- Attribute: schema-level metadata prefixed with `@`, e.g. `@name('User')`, `@timestamps()`.
- Registry: runtime storage for generated types (SchemaRegistry) and validators (ValidatorRegistry).
- Type emission: a TypeBuilderVisitor walks the AST and emits a readonly struct per TypeDefinition using System.Reflection.Emit. Each struct has private backing fields and public properties; a constructor takes all field values.
- Validation: preset validators exist (notEmpty, passwordValid, emailValid) and can be mapped per field type.

## How to define types
Example:

```
type User {
  @name('User')
  @description('Represents a system user')

  id {
    type(int)
    default(auto_increment)
    primary()
    null(false)
    comment('The user ID')
  }
  name {
    type(string)
    null(false)
    comment('The user's full name')
  }
  email {
    type(string)
    null(false)
    unique()
    validate(emailValid)
  }
  
  @timestamps()
}
```

- The type name is the generated struct name (suffixing is used to avoid collisions in tests).
- Each field results in a value-type member on the generated struct, enforcing a small memory footprint.
- Field properties define the field shape and constraints.

## How types are emitted
- Runtime emission uses Reflection.Emit to craft a public struct with:
  - Private readonly backing fields
  - Public properties with getters
  - A constructor with all fields as parameters
- Generated types are registered in SchemaRegistry and can be instantiated with CreateInstance(typeName, values...).
- To avoid cross-run collisions, generated type names include a per-run suffix (ensured by the TypeBuilder).

## Limitations
- In-memory only; no persistence to disk by default.
- No inheritance or interface implementation for generated types to maximize inlining and minimize allocations.
- Types exist only within the current AppDomain/process; not designed for cross-assembly or cross-domain sharing.
- Generated type naming must be managed to avoid collisions (we suffix names).
- Testing requires isolation because SchemaRegistry is a static/global registry.

## Errors and troubleshooting
- Duplicate type name within an assembly: occurs if a generated type with the same name already exists in the dynamic module. Use unique suffixing per run or reset state between tests.
- Parser errors: common are unknown tokens or mismatched expected tokens (e.g., expecting a type token vs identifier). Review the grammar productions for the right token kinds.
- Validator binding errors: if you register a validator for a specific type, ensure the delegate signature matches Func<T, bool> for that type. If you register a generic object-based validator, you may need to adapt how it’s invoked.
- Registry errors: if you call CreateInstance for an unregistered type, you’ll get an InvalidOperationException. Use SchemaRegistry.TryGetType first or inspect the registry contents.

## Debugging tips
- Inspect SchemaRegistry.GetAllTypeNames() to see what types have been emitted and registered.
- Inspect the generated type metadata in GeneratedTypeInfo (Fields, Constructor, TypeName).
- If a test mutates global state, wrap with a Collection fixture to serialize test execution.
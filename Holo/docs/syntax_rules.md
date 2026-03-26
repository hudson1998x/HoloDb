# Holo Query Language Syntax

A GraphQL-like query language for structured data filtering and retrieval.

## Overview

```
User { 
    id, 
    name, 
    email,
    creatorUser { 
        id, 
        name, 
        avatar 
    },
    userCount subQuery(
        User { count(id) },
        where {
            id { above 20, below 40 }
        }
    )
}
```

---

## Grammar Overview

| Construct | Syntax | Example |
|-----------|--------|---------|
| **Query** | One or more top-level nodes | `User { ... }` |
| **Named Block** | `Identifier { fields }` | `User { id, name }` |
| **Field** | Basic selection unit | `id`, `count(id)`, `name as n` |
| **Function Assignment** | `field func(args)` | `userCount subQuery(...)` |
| **Filter Field** | `identifier { expression }` | `id { above 20 }` |
| **Where Clause** | `where { fields }` | `where { status { in [...] } }` |

---

## Node Types

### QueryNode (Root)
- **Purpose**: Container for all top-level nodes in a query
- **Children**: Any sequence of `NamedBlockNode`, `FunctionAssignmentNode`, `IdentifierNode`, `LiteralNode`

```
QueryNode
├── NamedBlockNode (User { ... })
├── NamedBlockNode (Orders { ... })
└── FunctionAssignmentNode (userCount subQuery(...))
```

---

### NamedBlockNode
- **Syntax**: `Identifier { fields }`
- **Purpose**: Represents a named collection of fields (like an entity or object)
- **Properties**:
  - `Name`: `IdentifierNode` - the block name
  - `Fields`: `NodeList` - list of field nodes

**Example**:
```
creatorUser { id, name, avatar }
```
```
NamedBlockNode
├── Name: "creatorUser"
└── Fields: [IdentifierNode("id"), IdentifierNode("name"), IdentifierNode("avatar")]
```

---

### IdentifierNode
- **Purpose**: Represents a field name or identifier
- **Properties**:
  - `Value`: `Token` - the identifier text

**Example**: `id`, `name`, `email`, `creatorUser`

---

### FieldNode
- **Purpose**: Represents a simple field selection (alias for IdentifierNode in most cases)
- **Properties**:
  - `Value`: `Token`

---

### FunctionCallNode
- **Syntax**: `Identifier( arguments )`
- **Purpose**: Represents a function invocation
- **Properties**:
  - `FunctionName`: `IdentifierNode` - the function name
  - `Arguments`: `NodeList` - function arguments

**Example**:
```
count(id)
subQuery(User { count(id) }, where { ... })
```

---

### FunctionAssignmentNode
- **Syntax**: `Identifier Identifier( arguments )`
- **Purpose**: Assigns the result of a function call to a field name
- **Properties**:
  - `Field`: `IdentifierNode` - the target field name
  - `Function`: `FunctionCallNode` - the function being called

**Example**:
```
userCount subQuery(
    User { count(id) },
    where { id { above 20 } }
)
```
```
FunctionAssignmentNode
├── Field: "userCount"
└── Function: FunctionCallNode
    ├── FunctionName: "subQuery"
    └── Arguments: [NamedBlockNode, WhereClauseNode]
```

---

### AliasedNode
- **Syntax**: `field as alias`
- **Purpose**: Renames a field in the output
- **Properties**:
  - `Left`: `SyntaxNode` - the original field
  - `Alias`: `IdentifierNode` - the alias name

**Example**:
```
count(id) as total
```
```
AliasedNode
├── Left: FunctionCallNode(count(id))
└── Alias: "total"
```

---

### FilterFieldNode
- **Syntax**: `Identifier { expression }`
- **Purpose**: Applies a filter to a field
- **Properties**:
  - `Field`: `IdentifierNode` - the field being filtered
  - `Filter`: `SyntaxNode` - the filter expression

**Example**:
```
id { above 20, below 40 }
```
```
FilterFieldNode
├── Field: "id"
└── Filter: FilterListNode [FilterRuleNode(above, 20), FilterRuleNode(below, 40)]
```

---

### FilterRuleNode
- **Syntax**: `Operator Value`
- **Purpose**: A single filter condition
- **Properties**:
  - `Operator`: `IdentifierNode` - the comparison operator
  - `Value`: `SyntaxNode` - the value to compare against

**Operators**:
- `above` - greater than
- `below` - less than
- `in` - contained in array
- `contains` - string contains
- `startsWith` - string starts with
- `endsWith` - string ends with
- `is` - equality
- Custom identifiers

**Example**:
```
status in ['completed', 'dispatched']
above 20
```

---

### FilterListNode
- **Syntax**: `rule1, rule2, ...`
- **Purpose**: Multiple filter rules (implicit AND)
- **Properties**:
  - `Rules`: `NodeList` - list of `FilterRuleNode` or `FilterGroupNode`

**Example**:
```
above 20, below 40
```
```
FilterListNode
└── Rules: [FilterRuleNode(above, 20), FilterRuleNode(below, 40)]
```

---

### FilterBinaryNode
- **Syntax**: `left op right`
- **Purpose**: Logical combination of filter expressions
- **Properties**:
  - `Left`: `SyntaxNode` - left operand
  - `Operator`: `IdentifierNode` - "and" or "or"
  - `Right`: `SyntaxNode` - right operand

**Example**:
```
status is 'active' and id above 10
```

---

### FilterGroupNode
- **Syntax**: `( expression )`
- **Purpose**: Groups filter expressions with parentheses
- **Note**: The AST unwraps the group and returns the inner expression

**Example**:
```
(status is 'active' and id above 10)
```

---

### WhereClauseNode
- **Syntax**: `where { fields }`
- **Purpose**: Represents a WHERE clause containing filter fields
- **Properties**:
  - `Filters`: `NodeList` - list of filter field nodes

**Example**:
```
where {
    id { above 20, below 40 },
    status { in ['active', 'pending'] }
}
```

---

### LiteralNode
- **Purpose**: Represents a literal value
- **Properties**:
  - `Value`: `Token` - the literal token

**Types**:
- `StringLiteral`: `'hello world'`
- `NumberLiteral`: `42`, `3.14`
- `BooleanLiteral`: `true`, `false`
- `NullLiteral`: `null`

---

### ArrayLiteralNode
- **Syntax**: `[ item1, item2, ... ]`
- **Purpose**: Represents an array of values
- **Properties**:
  - `Items`: `NodeList` - list of literal nodes

**Example**:
```
['completed', 'dispatched']
```

---

### BinaryExpressionNode
- **Syntax**: `left op right`
- **Purpose**: Generic binary expression (used in filters)
- **Properties**:
  - `Left`: `SyntaxNode`
  - `Operator`: `IdentifierNode`
  - `Right`: `SyntaxNode`

---

### EmptyNode
- **Purpose**: Placeholder node for tokens that don't produce meaningful AST nodes (brackets, parens)

---

### NodeList
- **Purpose**: Container for a list of sibling nodes
- **Properties**:
  - `Items`: `List<SyntaxNode>`

---

## Keywords

| Keyword | TokenKind | Description |
|---------|-----------|-------------|
| `is` | `KeywordIs` | Equality operator |
| `not` | `KeywordNot` | Negation |
| `in` | `KeywordIn` | Membership operator |
| `where` | `KeywordWhere` | Where clause |
| `contains` | `KeywordContains` | String contains |
| `startsWith` | `KeywordStartsWith` | String prefix |
| `endsWith` | `KeywordEndsWith` | String suffix |
| `within` | `KeywordWithin` | Spatial/temporal within |
| `null` | `NullLiteral` | Null value |
| `true` / `false` | `BooleanLiteral` | Boolean values |

---

## Operators

| Symbol | TokenKind | Description |
|--------|-----------|-------------|
| `{` | `LeftBracket` | Block start |
| `}` | `RightBracket` | Block end |
| `(` | `LeftParenthesis` | Grouping / function args |
| `)` | `RightParenthesis` | Grouping / function args |
| `[` | `LeftSquareBracket` | Array start |
| `]` | `RightSquareBracket` | Array end |
| `,` | `Comma` | Separator |
| `<` | `LessThan` | Less than |
| `>` | `MoreThan` | Greater than |
| `=` | `Equal` | Equality |

---

## Extended Examples

### Full Complex Query

```
User { 
    id, 
    name, 
    email, 
    creatorUser { 
        id, 
        name, 
        avatar 
    }, 
    projects { 
        id, 
        title, 
        status 
    }, 
    userCount subQuery(
        User { count(id) }, 
        where {
            id { above 20, below 40 }
        }
    ),
    completedOrders subQuery( 
        Orders { count(id) as total },
        where {
            status { in ['completed', 'dispatched'] }
        }
    ) 
}
```

### AST Structure

```
QueryNode
├── NamedBlockNode ("User")
│   ├── IdentifierNode ("id")
│   ├── IdentifierNode ("name")
│   ├── IdentifierNode ("email")
│   ├── NamedBlockNode ("creatorUser")
│   │   ├── IdentifierNode ("id")
│   │   ├── IdentifierNode ("name")
│   │   └── IdentifierNode ("avatar")
│   ├── NamedBlockNode ("projects")
│   │   ├── IdentifierNode ("id")
│   │   ├── IdentifierNode ("title")
│   │   └── IdentifierNode ("status")
│   └── FunctionAssignmentNode ("userCount")
│       └── FunctionCallNode ("subQuery")
│           ├── Arguments: [NamedBlockNode("User"), WhereClauseNode]
│           └── ...
├── NamedBlockNode ("Orders") ...
```

---

## Error Handling

The parser throws `SyntaxErrorException` when:
- Unexpected token encountered
- Missing required tokens (e.g., unclosed braces)
- Invalid token sequence that doesn't match any production
- Premature end of input

The exception includes:
- The problematic token
- Source position information
- Original source text for context
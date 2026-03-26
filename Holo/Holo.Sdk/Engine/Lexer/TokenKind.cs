namespace Holo.Sdk.Engine.Lexer
{
    /// <summary>
    /// Represents the different types of tokens that can be produced by <see cref="QueryLexer"/>.
    /// </summary>
    public enum TokenKind : byte
    {
        // -------------------------
        // Structural symbols
        // -------------------------

        /// <summary>Left curly bracket '{'.</summary>
        LeftBracket, 

        /// <summary>Right curly bracket '}'.</summary>
        RightBracket, 

        /// <summary>Left parenthesis '('.</summary>
        LeftParenthesis, 

        /// <summary>Right parenthesis ')'.</summary>
        RightParenthesis, 

        /// <summary>Comma ','.</summary>
        Comma, 

        /// <summary>Left square bracket '['.</summary>
        LeftSquareBracket, 

        /// <summary>Right square bracket ']'.</summary>
        RightSquareBracket, 

        // -------------------------
        // Literals
        // -------------------------

        /// <summary>Identifier (variable or property name).</summary>
        Identifier, 

        /// <summary>Null literal ('null').</summary>
        NullLiteral, 

        /// <summary>Boolean literal ('true' or 'false').</summary>
        BooleanLiteral, 

        /// <summary>Numeric literal (integer or floating point).</summary>
        NumberLiteral, 

        /// <summary>String literal (single-quoted strings).</summary>
        StringLiteral, 

        // -------------------------
        // Keywords
        // -------------------------

        /// <summary>'is' keyword.</summary>
        KeywordIs, 

        /// <summary>'not' keyword.</summary>
        KeywordNot, 

        /// <summary>'within' keyword.</summary>
        KeywordWithin, 

        /// <summary>'contains' keyword.</summary>
        KeywordContains, 

        /// <summary>'startsWith' keyword.</summary>
        KeywordStartsWith, 

        /// <summary>'endsWith' keyword.</summary>
        KeywordEndsWith, 

        /// <summary>'in' keyword.</summary>
        KeywordIn, 
        
        /// <summary>'where' keyword</summary>
        KeywordWhere,

        /// <summary>'function' keyword for defining stored functions.</summary>
        KeywordFunction,

        /// <summary>'return' keyword - not allowed in function bodies.</summary>
        KeywordReturn,

        /// <summary>'type' keyword for defining schema types.</summary>
        KeywordType,

        /// <summary>'default' keyword for field default values.</summary>
        KeywordDefault,

        /// <summary>'primary' keyword for marking primary key fields.</summary>
        KeywordPrimary,

        /// <summary>'unique' keyword for marking unique constraint.</summary>
        KeywordUnique,

        /// <summary>'sensitive' keyword for marking sensitive fields.</summary>
        KeywordSensitive,

        /// <summary>'null' keyword for nullable field specification.</summary>
        KeywordNullable,

        /// <summary>'comment' keyword for field documentation.</summary>
        KeywordComment,

        /// <summary>'validate' keyword for field validation.</summary>
        KeywordValidate,

        /// <summary>'auto_increment' keyword for auto-incrementing fields.</summary>
        KeywordAutoIncrement,

        /// <summary>'int' keyword for integer type.</summary>
        KeywordInt,

        /// <summary>'string' keyword for string type.</summary>
        KeywordString,

        /// <summary>Dollar sign '$' used to prefix variable names in function declarations.</summary>
        DollarSign,

        /// <summary>At sign '@' used for schema attributes.</summary>
        At,

        // -------------------------
        // Operators
        // -------------------------

        /// <summary>Less-than operator '&lt;'.</summary>
        LessThan, 

        /// <summary>Greater-than operator '&gt;'.</summary>
        MoreThan, 

        /// <summary>Equality operator '='.</summary>
        Equal, 

        /// <summary>Colon ':' used for type annotations in function parameters.</summary>
        Colon, 

        // -------------------------
        // Special
        // -------------------------

        /// <summary>Represents a token that could not be correctly parsed or is invalid.</summary>
        Error
    }
}
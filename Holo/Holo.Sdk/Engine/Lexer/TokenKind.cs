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

        // -------------------------
        // Operators
        // -------------------------

        /// <summary>Less-than operator '&lt;'.</summary>
        LessThan, 

        /// <summary>Greater-than operator '&gt;'.</summary>
        MoreThan, 

        /// <summary>Equality operator '='.</summary>
        Equal, 

        // -------------------------
        // Special
        // -------------------------

        /// <summary>Represents a token that could not be correctly parsed or is invalid.</summary>
        Error
    }
}
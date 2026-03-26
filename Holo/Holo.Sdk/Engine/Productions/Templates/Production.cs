using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Base class for all parser productions.
    /// A production defines how a sequence of tokens should be matched
    /// and how an AST node should be built from those tokens.
    /// </summary>
    public abstract partial class Production
    {
        /// <summary>
        /// Attempts to parse this production from the given context.
        /// </summary>
        /// <param name="context">The parser context containing the token stream.</param>
        /// <returns>
        /// An <see cref="SyntaxNode"/> if parsing succeeds; otherwise, null.
        /// </returns>
        public abstract SyntaxNode? Parse(ref ParserContext context);
    }
}
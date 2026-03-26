using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Represents a parsing production rule that can be used to parse syntax nodes.
/// This partial class includes the <see cref="Choice"/> helper for selecting between multiple productions.
/// </summary>
public partial class Production
{
    /// <summary>
    /// Creates a production that attempts to parse using multiple options in order,
    /// returning the first successful match.
    /// </summary>
    /// <param name="options">An array of <see cref="Production"/> instances to try in order.</param>
    /// <returns>A <see cref="Production"/> that represents the choice between the given options.</returns>
    public static Production Choice(params Production[] options)
    {
        return new ChoiceProduction(options);
    }

    /// <summary>
    /// A production that attempts each option sequentially and returns the first successful parse.
    /// </summary>
    private sealed class ChoiceProduction : Production
    {
        private readonly Production[] _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceProduction"/> class
        /// with the given set of options.
        /// </summary>
        /// <param name="options">The array of productions to try.</param>
        public ChoiceProduction(Production[] options)
        {
            _options = options;
        }

        /// <summary>
        /// Attempts to parse the input using each production in order,
        /// returning the first successful <see cref="SyntaxNode"/> or <c>null</c> if none match.
        /// </summary>
        /// <param name="context">The parser context to read tokens from and manipulate.</param>
        /// <returns>The first successfully parsed <see cref="SyntaxNode"/> or <c>null</c> if no options succeed.</returns>
        public override SyntaxNode? Parse(ref ParserContext context)
        {
            int startPos = context.Position;

            foreach (var option in _options)
            {
                var node = option.Parse(ref context);
                if (node != null) return node; // first successful match
                context.Rewind(startPos);
            }

            return null; // none matched
        }
    }
}
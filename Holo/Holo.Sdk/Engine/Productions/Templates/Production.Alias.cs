using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public abstract partial class Production
{
    /// <summary>
    /// Assigns an alias to this production so that its resulting <see cref="SyntaxNode"/>
    /// can be captured when used within a sequence production.
    /// </summary>
    /// <param name="alias">The name to use as the alias for this production.</param>
    /// <returns>
    /// A new <see cref="Production"/> instance that wraps the original production with the given alias.
    /// </returns>
    public Production As(string alias)
    {
        return new AliasedProduction(this, alias);
    }

    /// <summary>
    /// Wraps a production and associates it with an alias.
    /// This is used by sequence productions to capture the result of the production
    /// under a specific name.
    /// </summary>
    private sealed class AliasedProduction : Production
    {
        /// <summary>
        /// The original production being aliased.
        /// </summary>
        public readonly Production Inner;

        /// <summary>
        /// The alias name assigned to the production.
        /// </summary>
        public readonly string Alias;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasedProduction"/> class
        /// that wraps an existing production with the given alias.
        /// </summary>
        /// <param name="inner">The production to wrap.</param>
        /// <param name="alias">The alias to assign to the production.</param>
        public AliasedProduction(Production inner, string alias)
        {
            Inner = inner;
            Alias = alias;
        }

        /// <summary>
        /// Attempts to parse the inner production.
        /// If successful, the node will be captured by its alias by the sequence production.
        /// </summary>
        /// <param name="context">Reference to the current parser context.</param>
        /// <returns>
        /// The parsed <see cref="SyntaxNode"/> if the inner production succeeds; otherwise, null.
        /// </returns>
        public override SyntaxNode? Parse(ref ParserContext context)
        {
            var node = Inner.Parse(ref context);
            if (node != null)
            {
                // Node will be captured by SequenceProduction via the alias
            }
            return node;
        }
    }
}
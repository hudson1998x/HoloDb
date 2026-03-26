using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial abstract class for parser productions, including helpers for common parsing patterns.
/// </summary>
public abstract partial class Production
{
    /// <summary>
    /// Creates a lazy production that defers evaluation until parse time.
    /// This is particularly useful for breaking circular references in grammar definitions.
    /// </summary>
    /// <param name="factory">
    /// A function that returns a <see cref="Production"/> when called.
    /// The factory is invoked only during parsing.
    /// </param>
    /// <returns>A <see cref="Production"/> that evaluates lazily.</returns>
    public static Production Lazy(Func<Production> factory)
    {
        return new LazyProduction(factory);
    }

    /// <summary>
    /// A production that evaluates another production lazily at parse time.
    /// </summary>
    private sealed class LazyProduction : Production
    {
        private readonly Func<Production> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyProduction"/> class.
        /// </summary>
        /// <param name="factory">The factory function to produce the actual production.</param>
        public LazyProduction(Func<Production> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Parses the input by invoking the factory to get the actual production
        /// and delegating parsing to it.
        /// </summary>
        /// <param name="context">The parser context.</param>
        /// <returns>
        /// The <see cref="SyntaxNode"/> returned by the lazily evaluated production,
        /// or <c>null</c> if parsing fails.
        /// </returns>
        public override SyntaxNode? Parse(ref ParserContext context)
        {
            var production = _factory();
            return production.Parse(ref context);
        }
    }
}
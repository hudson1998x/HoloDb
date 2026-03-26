using Xunit;

namespace Holo.Tests.Engine.Schema
{
    /// <summary>
    /// Collection definition to ensure schema tests run sequentially.
    /// This prevents race conditions with the static SchemaRegistry.
    /// </summary>
    [CollectionDefinition("SchemaTests", DisableParallelization = true)]
    public class SchemaTestsCollection
    {
    }
}

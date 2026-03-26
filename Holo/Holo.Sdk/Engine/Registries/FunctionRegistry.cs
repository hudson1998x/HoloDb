using System.Collections.Concurrent;

namespace Holo.Sdk.Engine.Registries;

/// <summary>
/// A thread-safe registry for storing and managing user-defined stored functions.
/// Functions can be registered, updated, retrieved, and removed at runtime.
/// </summary>
public static class FunctionRegistry
{
    private static readonly ConcurrentDictionary<string, StoredFunction> _functions = new();

    /// <summary>
    /// Registers or updates a stored function in the registry.
    /// </summary>
    /// <param name="function">The function to register.</param>
    public static void Register(StoredFunction function)
    {
        _functions[function.Name] = function;
    }

    /// <summary>
    /// Attempts to retrieve a function by its name.
    /// </summary>
    /// <param name="name">The name of the function to retrieve.</param>
    /// <param name="function">When this method returns, contains the function if found; otherwise, null.</param>
    /// <returns>True if the function was found; otherwise, false.</returns>
    public static bool TryGet(string name, out StoredFunction? function)
    {
        return _functions.TryGetValue(name, out function);
    }

    /// <summary>
    /// Removes a function from the registry by its name.
    /// </summary>
    /// <param name="name">The name of the function to remove.</param>
    /// <returns>True if the function was found and removed; otherwise, false.</returns>
    public static bool Remove(string name)
    {
        return _functions.TryRemove(name, out _);
    }

    /// <summary>
    /// Clears all functions from the registry.
    /// </summary>
    public static void Clear()
    {
        _functions.Clear();
    }

    /// <summary>
    /// Gets all function names currently registered.
    /// </summary>
    public static IEnumerable<string> GetAllFunctionNames() => _functions.Keys;
}

/// <summary>
/// Represents a stored function that can be invoked at runtime.
/// </summary>
public class StoredFunction
{
    /// <summary>
    /// Gets or sets the name of the function.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of parameter names in order.
    /// </summary>
    public required string[] Parameters { get; set; }

    /// <summary>
    /// Gets or sets the abstract syntax tree representing the function body.
    /// </summary>
    public required SyntaxTree.SyntaxNode BodyAST { get; set; }

    /// <summary>
    /// Gets or sets the mapping of parameter names to their .NET types.
    /// </summary>
    public required Dictionary<string, Type> ParameterTypes { get; set; }

    /// <summary>
    /// Gets or sets the return type of the function.
    /// </summary>
    public Type? ReturnType { get; set; }
}
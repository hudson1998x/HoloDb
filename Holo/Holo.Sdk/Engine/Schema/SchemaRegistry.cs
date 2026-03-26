using System.Collections.Concurrent;

namespace Holo.Sdk.Engine.Schema;

/// <summary>
/// Thread-safe registry for storing and managing dynamically generated schema types.
/// </summary>
public static class SchemaRegistry
{
    private static readonly ConcurrentDictionary<string, GeneratedTypeInfo> _types = new();

    /// <summary>
    /// Registers a generated type in the registry.
    /// </summary>
    /// <param name="typeInfo">The generated type information to register.</param>
    public static void Register(GeneratedTypeInfo typeInfo)
    {
        _types[typeInfo.TypeName] = typeInfo;
    }

    /// <summary>
    /// Attempts to retrieve a generated type by its name.
    /// </summary>
    /// <param name="typeName">The name of the type to retrieve.</param>
    /// <param name="typeInfo">When this method returns, contains the type info if found; otherwise, null.</param>
    /// <returns>True if the type was found; otherwise, false.</returns>
    public static bool TryGetType(string typeName, out GeneratedTypeInfo? typeInfo)
    {
        return _types.TryGetValue(typeName, out typeInfo);
    }

    /// <summary>
    /// Creates an instance of a registered schema type.
    /// </summary>
    /// <param name="typeName">The name of the schema type.</param>
    /// <param name="values">The field values in order.</param>
    /// <returns>A new instance of the requested type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the type is not registered.</exception>
    public static object CreateInstance(string typeName, params object[] values)
    {
        if (!_types.TryGetValue(typeName, out var typeInfo))
            throw new InvalidOperationException($"Schema type '{typeName}' is not registered.");

        return typeInfo.Constructor.Invoke(values);
    }

    /// <summary>
    /// Creates a strongly-typed instance of a registered schema type.
    /// </summary>
    /// <typeparam name="T">The expected type.</typeparam>
    /// <param name="typeName">The name of the schema type.</param>
    /// <param name="values">The field values in order.</param>
    /// <returns>A new instance cast to the specified type.</returns>
    public static T CreateInstance<T>(string typeName, params object[] values) where T : struct
    {
        return (T)CreateInstance(typeName, values);
    }

    /// <summary>
    /// Gets the generated type info for a schema type.
    /// </summary>
    /// <param name="typeName">The name of the schema type.</param>
    /// <returns>The generated type info, or null if not found.</returns>
    public static GeneratedTypeInfo? GetType(string typeName)
    {
        _types.TryGetValue(typeName, out var typeInfo);
        return typeInfo;
    }

    /// <summary>
    /// Gets all registered type names.
    /// </summary>
    public static IEnumerable<string> GetAllTypeNames() => _types.Keys;

    /// <summary>
    /// Removes a type from the registry.
    /// </summary>
    /// <param name="typeName">The name of the type to remove.</param>
    /// <returns>True if the type was found and removed; otherwise, false.</returns>
    public static bool Remove(string typeName)
    {
        return _types.TryRemove(typeName, out _);
    }

    /// <summary>
    /// Clears all registered types.
    /// </summary>
    public static void Clear()
    {
        _types.Clear();
    }

    /// <summary>
    /// Gets the total count of registered types.
    /// </summary>
    public static int Count => _types.Count;
}

using System.Reflection;

namespace Holo.Sdk.Engine.Schema;

/// <summary>
/// Registry for field validation functions.
/// Validators have the signature Func&lt;T, bool&gt; where T is the field type.
/// </summary>
public static class ValidatorRegistry
{
    private static readonly Dictionary<string, ValidatorDefinition> _validators = new();

    static ValidatorRegistry()
    {
        // Register built-in validators
        Register("notEmpty", typeof(string), value => !string.IsNullOrWhiteSpace((string)value));
        Register("passwordValid", typeof(string), value =>
        {
            var pwd = (string)value;
            return pwd.Length >= 8 &&
                   pwd.Any(char.IsUpper) &&
                   pwd.Any(char.IsLower) &&
                   pwd.Any(char.IsDigit);
        });
        Register("emailValid", typeof(string), value =>
        {
            var email = (string)value;
            return email.Contains('@') && email.Contains('.') && email.Length > 5;
        });
    }

    /// <summary>
    /// Registers a new validator function.
    /// </summary>
    /// <param name="name">The validator name (e.g., "notEmpty").</param>
    /// <param name="targetType">The type this validator applies to.</param>
    /// <param name="validator">The validation function.</param>
    public static void Register(string name, Type targetType, Func<object, bool> validator)
    {
        _validators[name] = new ValidatorDefinition
        {
            Name = name,
            TargetType = targetType,
            Validator = validator
        };
    }

    /// <summary>
    /// Tries to get a compiled validation delegate for the given validator name and field type.
    /// </summary>
    /// <param name="validatorName">The name of the validator.</param>
    /// <param name="fieldType">The type of the field being validated.</param>
    /// <returns>A compiled delegate, or null if the validator doesn't exist or doesn't match the type.</returns>
    public static Delegate? GetValidator(string validatorName, Type fieldType)
    {
        if (!_validators.TryGetValue(validatorName, out var definition))
            return null;

        if (definition.TargetType != fieldType)
            return null;

        // Create a strongly-typed delegate: Func<T, bool>
        var delegateType = typeof(Func<,>).MakeGenericType(fieldType, typeof(bool));
        return Delegate.CreateDelegate(delegateType, definition.Target, definition.Method);
    }

    /// <summary>
    /// Checks if a validator with the given name exists.
    /// </summary>
    public static bool Exists(string name) => _validators.ContainsKey(name);

    /// <summary>
    /// Gets all registered validator names.
    /// </summary>
    public static IEnumerable<string> GetAllValidatorNames() => _validators.Keys;
}

/// <summary>
/// Internal definition of a validator.
/// </summary>
internal sealed class ValidatorDefinition
{
    public required string Name { get; init; }
    public required Type TargetType { get; init; }
    public required Func<object, bool> Validator { get; init; }
    public object Target => Validator.Target!;
    public MethodInfo Method => Validator.Method;
}

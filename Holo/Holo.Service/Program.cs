using System.Text;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.Productions;
using Holo.Sdk.Engine.Schema;

var source = (
    """
    type User {
    
        @name('User')
        @description('This is the user entity, used for keeping track of users')
    
        id {
            type(int)
            default(auto_increment)
            primary()
            null(false)
            comment('The user ID')
        }
        name {
            type(string)
            null(false)
            comment('The users full name')
        }
        username {
            type(string)
            null(false)
            unique()
            validate(notEmpty)
            comment('The users unique username')
        }
        password {
            type(string)
            null(true)
            sensitive()
            validate(passwordValid)
            comment('The users hashed password')
        }
        
        email {
            type(string)
            null(false)
            unique()
            comment('The users unique email address')
            validate(emailValid)
        }
        
        @timestamps()
    }
    """);
    
var tokens = QueryLexer.Parse(source);

var node = Parser.Parse(tokens, source);
var builder = new StringBuilder();

node.DebugPrint(builder, source);

Console.WriteLine("====================================================");
Console.WriteLine("Tree preview");
Console.WriteLine("====================================================");
Console.WriteLine(builder.ToString());

Console.WriteLine();
Console.WriteLine("====================================================");
Console.WriteLine("Schema Types Generated");
Console.WriteLine("====================================================");

// Display generated types
foreach (var typeName in SchemaRegistry.GetAllTypeNames())
{
    var typeInfo = SchemaRegistry.GetType(typeName);
    if (typeInfo != null)
    {
        Console.WriteLine($"Type: {typeInfo.TypeName}");
        Console.WriteLine($"  .NET Type: {typeInfo.GeneratedType.FullName}");
        Console.WriteLine($"  Attributes:");
        foreach (var attr in typeInfo.TypeAttributes)
        {
            Console.WriteLine($"    @{attr.Key}({string.Join(", ", attr.Value)})");
        }
        Console.WriteLine($"  Fields:");
        foreach (var field in typeInfo.Fields)
        {
            var flags = new List<string>();
            if (field.IsPrimaryKey) flags.Add("PRIMARY KEY");
            if (field.IsNullable) flags.Add("NULLABLE");
            if (field.IsUnique) flags.Add("UNIQUE");
            if (field.IsSensitive) flags.Add("SENSITIVE");
            if (field.DefaultValue != null) flags.Add($"DEFAULT={field.DefaultValue}");
            if (field.ValidationDelegate != null) flags.Add("VALIDATED");
            
            Console.WriteLine($"    {field.Name}: {field.FieldType.Name} [{string.Join(", ", flags)}]");
            if (field.Comment != null)
                Console.WriteLine($"      // {field.Comment}");
        }
        Console.WriteLine();
    }
}

Console.WriteLine("====================================================");
Console.WriteLine("Creating Instance");
Console.WriteLine("====================================================");

// Create an instance of the User struct
var user = SchemaRegistry.CreateInstance("User", 1, "John Doe", "johndoe", "hashedpassword123", "john@example.com");

Console.WriteLine($"Created User instance: {user}");
Console.WriteLine($"Type: {user.GetType().FullName}");
Console.WriteLine($"Is ValueType: {user.GetType().IsValueType}");

// Access properties via reflection
var userType = SchemaRegistry.GetType("User")!;
foreach (var field in userType.Fields)
{
    var value = field.Property.GetValue(user);
    Console.WriteLine($"  {field.Name} = {value}");
}

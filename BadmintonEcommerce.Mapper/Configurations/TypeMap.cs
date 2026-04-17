using System.Linq.Expressions;

namespace BadmintonEcommerce.Mapper.Configurations;

public class TypeMap
{
    public Type SourceType { get; }
    public Type DestinationType { get; }

    public Dictionary<string, LambdaExpression> CustomMappings { get; } = new();
    public HashSet<string> IgnoredMembers { get; } = new();
    public Dictionary<string, LambdaExpression> Conditions { get; } = new();

    public Func<object, object> CompiledDelegate { get; set; }

    public TypeMap(Type source, Type destination)
    {
        SourceType = source;
        DestinationType = destination;
    }
}
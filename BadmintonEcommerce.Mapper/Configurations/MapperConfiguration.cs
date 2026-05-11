using System.Diagnostics.CodeAnalysis;
using BadmintonEcommerce.Mapper.Builders;

namespace BadmintonEcommerce.Mapper.Configurations;

[ExcludeFromCodeCoverage]
public class MapperConfiguration
{
    private readonly Dictionary<string, TypeMap> _maps = new();

    public MappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        var key = GetKey(typeof(TSource), typeof(TDestination));

        var map = new TypeMap(typeof(TSource), typeof(TDestination));
        _maps[key] = map;

        return new MappingExpression<TSource, TDestination>(map, this);
    }

    public TypeMap GetMap(Type source, Type destination)
    {
        var key = GetKey(source, destination);
        return _maps.TryGetValue(key, out var map) ? map : null;
    }

    private string GetKey(Type s, Type d) => $"{s.FullName}_{d.FullName}";
}
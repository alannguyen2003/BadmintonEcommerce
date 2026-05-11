using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Mapper.Runtime;

[ExcludeFromCodeCoverage]
public class Mapper : IMapper
{
    private readonly MapperConfiguration _config;

    public Mapper(MapperConfiguration config)
    {
        _config = config;
    }

    public TDestination Map<TDestination>(object source)
    {
        return (TDestination)Map(source, source.GetType(), typeof(TDestination));
    }

    public object Map(object source, Type sourceType, Type destinationType)
    {
        // 🔥 HANDLE COLLECTION FIRST
        if (IsCollection(sourceType) && IsCollection(destinationType))
        {
            var sourceElementType = GetElementType(sourceType);
            var destElementType = GetElementType(destinationType);

            var itemMap = _config.GetMap(sourceElementType, destElementType);

            if (itemMap == null)
                throw new Exception($"No mapping found: {sourceElementType} -> {destElementType}");

            if (itemMap.CompiledDelegate == null)
            {
                itemMap.CompiledDelegate = MappingExecutor.Build(itemMap, _config);
            }

            var method = typeof(Mapper)
                .GetMethod(nameof(MapCollectionInternal), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(sourceElementType, destElementType);

            return method.Invoke(this, new object[] { source, itemMap.CompiledDelegate });
        }
        
        
        var map = _config.GetMap(sourceType, destinationType);

        if (map == null)
            throw new Exception($"No mapping found: {sourceType} -> {destinationType}");

        if (map.CompiledDelegate == null)
        {
            map.CompiledDelegate = MappingExecutor.Build(map, _config);
        }

        return map.CompiledDelegate(source);
    }
    
    private List<TDestination> MapCollectionInternal<TSource, TDestination>(
        IEnumerable<TSource> source,
        Func<object, object> mapFunc)
    {
        if (source == null)
            return null;

        var list = new List<TDestination>();

        foreach (var item in source)
        {
            var mapped = (TDestination)mapFunc(item);
            list.Add(mapped);
        }

        return list;
    }
    private bool IsCollection(Type type)
    {
        return type != typeof(string) &&
               typeof(IEnumerable).IsAssignableFrom(type);
    }

    private Type GetElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        if (type.IsGenericType)
            return type.GetGenericArguments()[0];

        return typeof(object);
    }
}
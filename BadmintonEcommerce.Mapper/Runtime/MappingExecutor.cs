using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Mapper.Runtime;

public static class MappingExecutor
{
    public static Func<object, object> Build(TypeMap map, MapperConfiguration config)
    {
        var sourceParam = Expression.Parameter(typeof(object), "src");
        var sourceCast = Expression.Convert(sourceParam, map.SourceType);

        var bindings = new List<MemberBinding>();

        foreach (var destProp in map.DestinationType.GetProperties())
        {
            if (map.IgnoredMembers.Contains(destProp.Name))
                continue;

            Expression valueExpr = null;

            // Custom mapping
            if (map.CustomMappings.TryGetValue(destProp.Name, out var custom))
            {
                valueExpr = Expression.Invoke(custom, sourceCast);
            }
            else
            {
                var sourceProp = map.SourceType.GetProperty(destProp.Name);

                if (sourceProp != null)
                {
                    var propExpr = Expression.Property(sourceCast, sourceProp);
                    // 🔥 COLLECTION HANDLING
                    if (IsCollection(destProp.PropertyType))
                    {
                        var sourceElementType = GetElementType(sourceProp.PropertyType);
                        var destElementType = GetElementType(destProp.PropertyType);

                        var nestedMap = config.GetMap(sourceElementType, destElementType);

                        if (nestedMap == null)
                        {
                            valueExpr = propExpr; // fallback
                        }
                        else
                        {
                            if (nestedMap.CompiledDelegate == null)
                            {
                                nestedMap.CompiledDelegate = Build(nestedMap, config);
                            }

                            var mapMethod = typeof(MappingExecutor)
                                .GetMethod(nameof(MapCollection), BindingFlags.NonPublic | BindingFlags.Static)
                                .MakeGenericMethod(sourceElementType, destElementType);
                            
                            valueExpr = Expression.Call(
                                mapMethod,
                                Expression.Constant(nestedMap.CompiledDelegate),
                                Expression.Convert(propExpr, typeof(IEnumerable<>).MakeGenericType(sourceElementType))
                            );

                            valueExpr = Expression.Convert(valueExpr, destProp.PropertyType);
                        }
                    }
                    // Nested mapping
                    else if (!IsPrimitive(destProp.PropertyType))
                    {
                        var nestedMap = config.GetMap(sourceProp.PropertyType, destProp.PropertyType);

                        if (nestedMap != null)
                        {
                            var nestedFunc = Build(nestedMap, config);

                            valueExpr = Expression.Invoke(
                                Expression.Constant(nestedFunc),
                                Expression.Convert(propExpr, typeof(object))
                            );

                            valueExpr = Expression.Convert(valueExpr, destProp.PropertyType);
                        }
                        else
                        {
                            valueExpr = propExpr;
                        }
                    }
                    else
                    {
                        valueExpr = propExpr;
                    }
                }
            }

            // Condition
            if (map.Conditions.TryGetValue(destProp.Name, out var cond))
            {
                var condExpr = Expression.Invoke(cond, sourceCast);
                valueExpr = Expression.Condition(
                    condExpr,
                    valueExpr,
                    Expression.Default(destProp.PropertyType)
                );
            }

            if (valueExpr != null)
            {
                valueExpr = EnsureType(valueExpr, destProp.PropertyType);
                bindings.Add(Expression.Bind(destProp, valueExpr));            }
        }

        var body = Expression.MemberInit(
            Expression.New(map.DestinationType),
            bindings
        );

        var lambda = Expression.Lambda<Func<object, object>>(
            Expression.Convert(body, typeof(object)),
            sourceParam
        );

        return lambda.Compile();
    }

    private static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid);
    }
    
    private static Expression EnsureType(Expression expr, Type targetType)
    {
        if (expr.Type == targetType)
            return expr;

        // Handle nullable
        if (Nullable.GetUnderlyingType(targetType) != null)
        {
            var underlying = Nullable.GetUnderlyingType(targetType);
            if (expr.Type == underlying)
            {
                return Expression.Convert(expr, targetType);
            }
        }

        return Expression.Convert(expr, targetType);
    }
    
    private static bool IsCollection(Type type)
    {
        return type != typeof(string) &&
               typeof(IEnumerable).IsAssignableFrom(type);
    }
    
    private static Type GetElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        if (type.IsGenericType)
            return type.GetGenericArguments()[0];

        return typeof(object);
    }
    
    private static List<TDestination> MapCollection<TSource, TDestination>(
        Func<object, object> mapFunc,
        IEnumerable<TSource> source)
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
}
using System.Linq.Expressions;
using BadmintonEcommerce.Mapper.Configurations;
using BadmintonEcommerce.Mapper.Utils;

namespace BadmintonEcommerce.Mapper.Builders;

public class MappingExpression<TSource, TDestination>
{
    private readonly TypeMap _map;
    private readonly MapperConfiguration _config;

    public MappingExpression(TypeMap map, MapperConfiguration config)
    {
        _map = map;
        _config = config;
    }

    public MappingExpression<TSource, TDestination> ForMember(
        Expression<Func<TDestination, object>> dest,
        Expression<Func<TSource, object>> src)
    {
        var destName = ExpressionHelper.GetMemberName(dest);
        _map.CustomMappings[destName] = src;
        return this;
    }

    public MappingExpression<TSource, TDestination> Ignore(
        Expression<Func<TDestination, object>> dest)
    {
        var name = ExpressionHelper.GetMemberName(dest);
        _map.IgnoredMembers.Add(name);
        return this;
    }

    public MappingExpression<TSource, TDestination> Condition(
        Expression<Func<TDestination, object>> dest,
        Expression<Func<TSource, bool>> condition)
    {
        var name = ExpressionHelper.GetMemberName(dest);
        _map.Conditions[name] = condition;
        return this;
    }

    public void ReverseMap()
    {
        _config.CreateMap<TDestination, TSource>();
    }
}
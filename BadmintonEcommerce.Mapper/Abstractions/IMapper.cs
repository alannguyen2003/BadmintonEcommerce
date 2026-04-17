namespace BadmintonEcommerce.Mapper.Abstractions;

public interface IMapper
{
    TDestination Map<TDestination>(object source);
    object Map(object source, Type sourceType, Type destinationType);
}
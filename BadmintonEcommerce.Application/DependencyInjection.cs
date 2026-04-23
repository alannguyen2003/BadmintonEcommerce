using System.Reflection;
using BadmintonEcommerce.Application.Abstraction.Profile;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Mapper.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonEcommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddCustomMapper();

    public static IServiceCollection AddCustomMapper(this IServiceCollection services)
    {
        var config = new MapperConfiguration();

        var profiles = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IMappingProfile).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var profile in profiles)
        {
            var instance = (IMappingProfile)Activator.CreateInstance(profile);
            instance.Configure(config);
        }

        services.AddSingleton<IMapper>(new global::BadmintonEcommerce.Mapper.Runtime.Mapper(config));

        return services;
    }
}
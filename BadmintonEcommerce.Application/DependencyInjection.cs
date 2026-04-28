using System.Reflection;
using BadmintonEcommerce.Application.Abstraction.Behaviours;
using BadmintonEcommerce.Application.Abstraction.Messaging;
/*
using BadmintonEcommerce.Application.Abstraction.Profile;
*/
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Mapper.Configurations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions;

namespace BadmintonEcommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        /*services
            .AddCustomMapper();*/

        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: true)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: true)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: true)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));

        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: true)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }

    /*public static IServiceCollection AddCustomMapper(this IServiceCollection services)
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
    }*/
}
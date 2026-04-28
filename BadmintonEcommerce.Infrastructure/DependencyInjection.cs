using System.Reflection;
using System.Text;
using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Abstraction.Services;
using BadmintonEcommerce.Infrastructure.Abstractions;
using BadmintonEcommerce.Infrastructure.DomainEvents;
using BadmintonEcommerce.Infrastructure.Persistence.Database;
using BadmintonEcommerce.Infrastructure.Persistence.Profiles;
using BadmintonEcommerce.Infrastructure.Repositories;
using BadmintonEcommerce.Infrastructure.Services;
using BadmintonEcommerce.Infrastructure.Time;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Mapper.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Services;

namespace BadmintonEcommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddRepositories()
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddCloudinary(configuration)
            .AddThirdPartyServices()
            .AddCustomMapper();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(Constant.Connection.Database.DefaultConnection);

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(connectionString));
        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        Console.WriteLine(configuration.GetConnectionString(Constant.Connection.Database.DefaultConnection));
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString(Constant.Connection.Database.DefaultConnection) ?? string.Empty);
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("JWT FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("JWT OK");
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                };
            });
        services.AddHttpContextAccessor();
        
        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySetting>(configuration.GetSection("Cloudinary"));
        return services;
    }

    private static IServiceCollection AddThirdPartyServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        return services;
    }
    
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
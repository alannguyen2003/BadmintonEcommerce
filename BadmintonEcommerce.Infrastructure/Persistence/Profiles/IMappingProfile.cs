using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles;

public interface IMappingProfile
{
    void Configure(MapperConfiguration configuration);
}
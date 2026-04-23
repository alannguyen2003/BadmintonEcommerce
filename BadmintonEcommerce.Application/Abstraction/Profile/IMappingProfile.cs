using BadmintonEcommerce.Mapper.Configurations;

namespace BadmintonEcommerce.Application.Abstraction.Profile;

public interface IMappingProfile
{
    void Configure(MapperConfiguration configuration);
}
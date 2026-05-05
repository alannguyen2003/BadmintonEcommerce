using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Application.Features.Authentication.Register;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Mapper.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonEcommerce.Infrastructure.Persistence.Profiles.Authentication.Request;

public class AccountProfile() : IMappingProfile
{
    public void Configure(MapperConfiguration configuration)
    {
        
        configuration.CreateMap<RegisterCommand, Account>()
            .ForMember(des => des.Email,
                src => src.Email)
            .ForMember(des => des.FullName,
                src => src.Fullname)
            .ForMember(des => des.Username,
                src => src.Username);

        configuration.CreateMap<RegisterRequest, RegisterCommand>()
            .ForMember(des => des.Email,
                src => src.Email)
            .ForMember(des => des.Fullname,
                src => src.Fullname)
            .ForMember(des => des.Username,
                src => src.Username)
            .ForMember(des => des.Password,
                src => src.Password);

        configuration.CreateMap<SignInRequest, LoginCommand>()
            .ForMember(des => des.Email,
                src => src.Email)
            .ForMember(des => des.Password,
                src => src.Password);
    }
}
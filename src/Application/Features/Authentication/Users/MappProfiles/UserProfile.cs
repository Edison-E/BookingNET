using BookPro.Application.Features.Authentication.Users.DTO;

namespace BookPro.Application.Features.Authentication.Users.MappProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
    }
}

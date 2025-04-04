namespace BookPro.Application.Features.Account.MappProfiles;

public class RegisterProfile : Profile
{
    public RegisterProfile()
    {
        CreateMap<RegisterRequest, User>();
    }
}

using BookPro.Application.Features.Authentication.Users.DTO;

namespace BookPro.Application.Features.Authentication.Users.Interface;

public interface IUserService
{
    Task<UserResponseDTO> GetUser(string email);
}

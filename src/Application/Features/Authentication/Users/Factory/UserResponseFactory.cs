using BookPro.Application.Features.Authentication.Users.DTO;

namespace BookPro.Application.Features.Authentication.Users.Factory;

public static class UserResponseFactory
{
    public static UserResponseDTO CreateUserResponse(List<string> message, UserDTO user, bool success) =>
         new UserResponseDTO(success, message, user);
}

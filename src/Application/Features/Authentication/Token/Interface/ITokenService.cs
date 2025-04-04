namespace BookPro.Application.Features.Token.Interface;

public interface ITokenService
{
    Task<TokenResponseDTO> GetAccessToken(User user);
    ClaimsPrincipal ValidateResetToken(string token);
}

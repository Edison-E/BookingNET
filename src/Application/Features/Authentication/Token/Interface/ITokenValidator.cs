namespace BookPro.Application.Features.Token.Interface;

public interface ITokenValidator
{
    RefreshTokenValidationResult ValidationRefreshToken(RefreshToken refreshToken);
    ValidationResult ValidationStringToken(string accessToken);
}

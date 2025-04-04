namespace BookPro.Application.Features.Token.Interface;

public interface IRefreshTokenValidator
{
    RefreshTokenValidationResult ValidateRefreshToken(RefreshToken token);
}

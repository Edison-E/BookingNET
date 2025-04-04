namespace BookPro.Application.Features.Token.Validations;

public class TokenValidator : ITokenValidator
{
    private readonly IStringTokenValidator _stringTokenValidator;
    private readonly IRefreshTokenValidator _refreshTokenValidator;

    public TokenValidator(IStringTokenValidator stringTokenValidator, IRefreshTokenValidator refreshTokenValidator)
    {
        _stringTokenValidator = stringTokenValidator;
        _refreshTokenValidator = refreshTokenValidator;
    }

    public ValidationResult ValidationStringToken(string accessToken)
    {
        return _stringTokenValidator.ValidateToken(accessToken);
    }

    public RefreshTokenValidationResult ValidationRefreshToken(RefreshToken refreshToken)
    {
        return _refreshTokenValidator.ValidateRefreshToken(refreshToken);
    }
}

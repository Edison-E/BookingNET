namespace BookPro.Application.Features.Token.Validations;

public class RefreshTokenValidator : BaseValidation<RefreshToken>, IRefreshTokenValidator
{
    public RefreshTokenValidationResult ValidateRefreshToken(RefreshToken token)
    {
        RefreshTokenValidationResult result = new RefreshTokenValidationResult();
        result.Message.AddRange(ValidateParameterRequiredString(token.Token, "Token").Message);
        result.Message.AddRange(ValidateParameterRequiredString(token.Id.ToString(), "Id").Message);
        result.Message.AddRange(ValidateParameterRequiredString(token.Expires.ToString(), "Expires").Message);
        result.Message.AddRange(ValidateParameterRequiredString(token.Created.ToString(), "Created").Message);
        result.Message.AddRange(ValidateParameterRequiredString(token.CreatedByIp, "CreatedByIp").Message);
        result.Message.AddRange(ValidateParameterRequiredString(token.UserId.ToString(), "UserId").Message);

        result.IsExpired = token.IsExpired;
        return result;
    }

}

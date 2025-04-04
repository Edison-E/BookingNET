namespace BookPro.Application.Features.Token.Validations;

public class StringTokenValidator : BaseValidation<string>, IStringTokenValidator
{

    public ValidationResult ValidateToken(string token)
    {
        ValidationResult result = new ValidationResult();
        result.Message.AddRange(ValidateEntity(token, "Token").Message);

        return result;
    }
}

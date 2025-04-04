namespace BookPro.Application.Features.Token.Interface;

public interface IStringTokenValidator
{
    ValidationResult ValidateToken(string token);
}

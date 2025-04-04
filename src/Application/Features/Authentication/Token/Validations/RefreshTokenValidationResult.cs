namespace BookPro.Application.Features.Token.Validations;

public class RefreshTokenValidationResult : ValidationResult
{
    public bool IsExpired { get; set; }
}

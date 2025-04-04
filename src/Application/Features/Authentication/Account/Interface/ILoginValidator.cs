namespace BookPro.Application.Features.Account.Interface;

public interface ILoginValidator : IValidator<LoginRequest>, IValidationFormat
{
    ValidationResult ValidLogin(LoginRequest loginDTO);
    ValidationResult ValidationCredentials(LoginRequest login, User user);
}

namespace BookPro.Application.Features.Account.Interface;

public interface IAccountValidator
{
    ValidationResult ValidationLogin(LoginRequest login);
    ValidationResult ValidationRegister(RegisterRequest register);
    ValidationResult ValidationCredentials(LoginRequest login, User user);
}

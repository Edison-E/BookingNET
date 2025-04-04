namespace BookPro.Application.Features.Account.Validations;

public class AccountValidator : IAccountValidator
{
    private readonly ILoginValidator _loginValidator;
    private readonly IRegisterValidator _registerValidator;

    public AccountValidator(ILoginValidator loginValidator, IRegisterValidator registerValidator)
    {
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
    }
    public ValidationResult ValidationLogin(LoginRequest login)
    {
        return _loginValidator.ValidLogin(login);
    }
    public ValidationResult ValidationRegister(RegisterRequest register)
    {
        return _registerValidator.ValidationRegister(register);
    }
    public ValidationResult ValidationCredentials(LoginRequest login, User user)
    {
        return _loginValidator.ValidationCredentials(login, user);
    }


}

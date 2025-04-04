namespace BookPro.Application.Features.Account.Interface;

public interface IRegisterValidator : IValidator<RegisterRequest>, IValidationFormat
{
    ValidationResult ValidationRegister(RegisterRequest registerDTO);
    ValidationResult ValidationDateBirth(string dateBirth);
    ValidationResult ValidationPhoneNumber(int phoneNumber);
}

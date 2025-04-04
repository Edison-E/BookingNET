namespace BookPro.Application.Validation.Interface;

public interface IValidationFormat
{
    ValidationResult ValidateEmailFormat(string email, string name);
    ValidationResult ValidatePasswordFormat(string Password, string name);

}

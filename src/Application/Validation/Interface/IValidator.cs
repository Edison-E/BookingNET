namespace BookPro.Application.Validation.Interface;

public interface IValidator<T> where T : class
{
    ValidationResult ValidateEntity(T entity, string name);
    ValidationResult ValidateParameterRequiredString(string value, string name);
    ValidationResult ValidateParameterRequiredInteger(int value, string name);
}

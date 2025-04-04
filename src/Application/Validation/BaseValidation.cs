namespace BookPro.Application.Validation;

public abstract class BaseValidation<T> : IValidator<T> where T : class
{
    public ValidationResult ValidateEntity(T entity, string name)
    {
        ValidationResult result = new ValidationResult();

        if (entity == null || entity == "")
        {
            result.Message.Add($"{name} is null");
        }

        return result;
    }

    public ValidationResult ValidateParameterRequiredInteger(int value, string name)
    {
        ValidationResult result = new ValidationResult();

        if (value == 0)
        {
            result.Message.Add($"{name} is required.");
        }

        return result;
    }

    public ValidationResult ValidateParameterRequiredString(string value, string name)
    {
        ValidationResult result = new ValidationResult();

        if (string.IsNullOrEmpty(value))
        {
            result.Message.Add($"{name} is required.");
        }

        return result;
    }

}

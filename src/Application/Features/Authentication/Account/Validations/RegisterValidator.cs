namespace BookPro.Application.Features.Account.Validations;

public class RegisterValidator : BaseValidation<RegisterRequest>, IRegisterValidator
{
    public ValidationResult ValidationRegister(RegisterRequest registerDTO)
    {
        ValidationResult result = new ValidationResult();
        result.Message.AddRange(ValidateEntity(registerDTO, "Data of the user ").Message);

        result.Message.AddRange(ValidateParameterRequiredString(registerDTO.Email, "Email").Message);
        result.Message.AddRange(ValidateParameterRequiredString(registerDTO.DateBirth, "Date of birth").Message);
        result.Message.AddRange(ValidateParameterRequiredString(registerDTO.PhoneNumber.ToString(), "Phone number").Message);
        result.Message.AddRange(ValidateParameterRequiredString(registerDTO.Name, "Name").Message);
        result.Message.AddRange(ValidateParameterRequiredString(registerDTO.Password, "Password").Message);

        result.Message.AddRange(ValidateEmailFormat(registerDTO.Email, "Email").Message);
        result.Message.AddRange(ValidationDateBirth(registerDTO.DateBirth).Message);
        result.Message.AddRange(ValidationPhoneNumber(registerDTO.PhoneNumber).Message);

        return result;
    }
    public ValidationResult ValidationDateBirth(string dateBirth)
    {
        ValidationResult result = new ValidationResult();

        if (DateTime.TryParse(dateBirth, out DateTime parsedDate))
        {
            var today = DateTime.Today;
            var age = today.Year - parsedDate.Year;

            if (parsedDate.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                result.Message.Add("User must be at least 18 years old.");
            }
        }
        else
        {
            result.Message.Add("Invalid date format for DateBirth");
        }

        return result;
    }

    public ValidationResult ValidatePasswordFormat(string emai, string name)
    {
        throw new NotImplementedException();
    }

    public ValidationResult ValidateEmailFormat(string email, string name)
    {
        ValidationResult result = new ValidationResult();

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            result.Message.Add($"{name} is not a valid email format.");
        }

        return result;

    }

    public ValidationResult ValidationPhoneNumber(int phoneNumber)
    {
        ValidationResult result = new ValidationResult();
        string numberString = phoneNumber.ToString();

        if (numberString.Length < 9 || numberString.Length > 12)
        {
            result.Message.Add("Phone number must be between 10 and 15 digits.");
        }

        return result;
    }

}

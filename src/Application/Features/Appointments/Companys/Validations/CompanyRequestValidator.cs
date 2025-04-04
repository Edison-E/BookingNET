using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Interface;

namespace BookPro.Application.Features.Appointments.Companys.Validations;

public class CompanyRequestValidator : BaseValidation<CompanyRequestDTO>, ICompanyRequestValidator
{
    public ValidationResult ValidCompanyRequest(CompanyRequestDTO request)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateEntity(request, "Company request").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Name, "Name company").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(request.PhoneNumber, "Phone number").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(request.Address.PostalCode, "Postal Code").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Address.Street, "Street").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Address.Country, "Country").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Address.City, "City").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.TypeService, "Service").Message);
        
        return result;
    }
}

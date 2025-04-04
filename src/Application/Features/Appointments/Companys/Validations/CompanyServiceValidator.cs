using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Validations;

public class CompanyServiceValidator : BaseValidation<TypeServices>, ICompanyServiceValidator
{
    public ValidationResult ValidateService(TypeServices service)
    {
        ValidationResult result= new ValidationResult();

        result.Message.AddRange(ValidateEntity(service, "Service").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(service.Id, "Id service").Message);
        result.Message.AddRange(ValidateParameterRequiredString(service.Name, "Name service").Message);

        return result;
    }
}

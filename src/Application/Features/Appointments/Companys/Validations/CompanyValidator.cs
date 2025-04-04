using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Validations;

public class CompanyValidator : BaseValidation<Domain.Entitys.Appointment.Company> ,ICompanyValidator
{
    private readonly ICompanyRequestValidator _requestValidator;
    private readonly ICompanyAddressValidator _addressValidator;
    private readonly ICompanyServiceValidator _serviceValidator;

    public CompanyValidator (
        ICompanyRequestValidator requestValidator, 
        ICompanyAddressValidator addressValidator,
        ICompanyServiceValidator serviceValidator)
    {
        _requestValidator = requestValidator;
        _addressValidator = addressValidator;
        _serviceValidator = serviceValidator;
    }

    public ValidationResult ValidationCompany(Domain.Entitys.Appointment.Company company)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateEntity(company, "Company").Message);
        result.Message.AddRange(ValidateParameterRequiredString(company.Name, "Name company").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(company.PhoneNumber, "Phone number").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(company.AddressId, "Address id").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(company.ServiceId, "Service id").Message);

        return result;
    }

    public ValidationResult ValidationCompanyRequest(CompanyRequestDTO request) 
        => _requestValidator.ValidCompanyRequest(request);

    public ValidationResult ValidationService(TypeServices services)
        => _serviceValidator.ValidateService(services);
    public ValidationResult ValidationAddress(Address address)
        => _addressValidator.ValidateAddress(address);

    public ValidationResult ValidationAddressRequest(AddressDTO address)
        => _addressValidator.ValidateAddressRequest(address);
    
}

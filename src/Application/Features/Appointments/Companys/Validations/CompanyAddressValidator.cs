using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Validations;

public class CompanyAddressValidator : BaseValidation<Address>, ICompanyAddressValidator
{
    public ValidationResult ValidateAddress(Address address)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateEntity(address, "Address").Message);
        result.Message.AddRange(ValidateParameterRequiredInteger(address.PostalCode, "Postal code").Message);
        result.Message.AddRange(ValidateParameterRequiredString(address.City, "City").Message);
        result.Message.AddRange(ValidateParameterRequiredString(address.Country, "Country").Message);
        result.Message.AddRange(ValidateParameterRequiredString(address.Street, "Street").Message);

        return result;
    }

    public ValidationResult ValidateAddressRequest(AddressDTO request)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateParameterRequiredInteger(request.PostalCode, "Postal code").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.City, "City").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Country, "Country").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Street, "Street").Message);

        return result;
    }
}

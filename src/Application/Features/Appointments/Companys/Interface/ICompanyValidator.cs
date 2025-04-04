using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyValidator
{
    ValidationResult ValidationCompany(Domain.Entitys.Appointment.Company company);
    ValidationResult ValidationCompanyRequest(CompanyRequestDTO request);
    ValidationResult ValidationAddress(Address address);
    ValidationResult ValidationAddressRequest(AddressDTO address);
    ValidationResult ValidationService(TypeServices services);
}

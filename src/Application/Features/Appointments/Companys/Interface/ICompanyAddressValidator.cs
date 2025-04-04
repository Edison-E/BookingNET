using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyAddressValidator
{
    ValidationResult ValidateAddress(Address address);
    ValidationResult ValidateAddressRequest(AddressDTO request);
}

using BookPro.Application.Features.Appointments.Companys.DTOs;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyRequestValidator
{
    ValidationResult ValidCompanyRequest(CompanyRequestDTO loginDTO);
}

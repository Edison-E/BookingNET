using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyServiceValidator
{
    ValidationResult ValidateService(TypeServices service);
}

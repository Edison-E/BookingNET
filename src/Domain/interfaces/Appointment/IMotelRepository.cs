using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface IMotelRepository
{
   Task<Motel> GetByCompany(int companyId);
}

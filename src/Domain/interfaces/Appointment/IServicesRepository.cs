using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface IServicesRepository
{
    Task<TypeServices> GetServiceByName(string name);
}

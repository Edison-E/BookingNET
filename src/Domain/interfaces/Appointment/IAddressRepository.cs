using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface IAddressRepository
{
    Task<bool> InsertAddress(Address address);
    Task<Address> GetAddressbyStreet(string street);
}

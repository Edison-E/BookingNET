using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Address> GetAddressbyStreet(string street)
        => await _context.Addresses.Where(x => x.Street == street).FirstOrDefaultAsync();

    public Task<bool> InsertAddress(Address address) => base.Insert(address);
}

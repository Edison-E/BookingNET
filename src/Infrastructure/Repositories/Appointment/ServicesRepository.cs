using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class ServicesRepository : Repository<TypeServices>, IServicesRepository
{
    public ServicesRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<TypeServices> GetServiceByName(string name)
    {
        return _context.TypeServices.Where(x => x.Name == name).FirstAsync();
    }
}

using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class MotelRepository : Repository<Motel> , IMotelRepository
{
    public MotelRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Motel> GetByCompany(int companyId) =>
        await _context.Motels.Where(x => x.CompanyId == companyId).FirstAsync();
}

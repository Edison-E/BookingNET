using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class AppointmentRepository : Repository<Domain.Entitys.Appointment.Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Domain.Entitys.Appointment.Appointment> GetLastAppointmentByUser(int userId)
    {
        return await _context.Appointments
            .Where(app => app.CustomerId == userId)
            .Include(app => app.Company)
            .ThenInclude(company => company.Address)
            .OrderByDescending(x => x.Created)
            .FirstAsync();
    }

    public async Task<bool> InsertAppointment(Domain.Entitys.Appointment.Appointment appointment) => await Insert(appointment);
}

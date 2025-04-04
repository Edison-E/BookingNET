using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface IAppointmentRepository
{
    Task<bool> InsertAppointment(Entitys.Appointment.Appointment appointment);
    Task<Entitys.Appointment.Appointment> GetLastAppointmentByUser(int userId);
}

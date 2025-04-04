namespace BookPro.Application.Features.Appointments.Bookings.Interface;

public interface IAppointmentValidation
{
    ValidationResult ValidateAppointment(AppointmentDTO appointment);
}

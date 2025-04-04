using BookPro.Application.Features.Appointments.Bookings.Response;

namespace BookPro.Application.Features.Appointments.Bookings.Interface;

public interface IBookingService
{
    Task<AppointmentResponseBase> Appointment(AppointmentBaseRequest request);
}

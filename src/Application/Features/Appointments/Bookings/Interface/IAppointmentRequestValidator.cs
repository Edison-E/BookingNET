using BookPro.Application.Features.Appointments.Companys.DTOs;

namespace BookPro.Application.Features.Appointments.Bookings.Interface;

public interface IAppointmentRequestValidator
{
    ValidationResult ValidateRequest(AppointmentBaseRequest request);
    ValidationResult ValidateCheckInCheckOut(MotelRequestDTO request);
}

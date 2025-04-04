using BookPro.Application.Features.Appointments.Bookings.Response;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Bookings.Interface;

public interface IAppointmentResponseFactory
{
    AppointmentResponse CreateSuccessAppointmentResponse(Appointment appointment);
    AppointmentResponse CreateErrorAppointmentResponse(AppointmentBaseRequest request, string status);
    MotelResponse CreateSuccessMotelResponse(MotelRequestDTO request, AppointmentResponse appointment);
    MotelResponse CreateErrorMotelResponse(MotelRequestDTO request, string status);
}

using BookPro.Application.Features.Appointments.Bookings.Response;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Domain.Entitys.Appointment;
using System.Globalization;

namespace BookPro.Application.Features.Appointments.Bookings.Factory;

public class AppointmentResponseFactory: IAppointmentResponseFactory
{
    public AppointmentResponse CreateErrorAppointmentResponse(AppointmentBaseRequest request, string status)
       => new AppointmentResponse(
         status, 
         0, 
         DateTime.ParseExact(request.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture), 
         TimeSpan.ParseExact(request.Hour, @"hh\:mm", CultureInfo.InvariantCulture), 
         request.CustomerEmail, 
         request.CompanyName, 
         request.Address);

     public AppointmentResponse CreateSuccessAppointmentResponse(Appointment appointment)
        => new AppointmentResponse(
            appointment.Status, 
            appointment.Id, 
            appointment.Date, 
            appointment.Hour, 
            appointment.User.Email, 
            appointment.Company.Name, 
            appointment.Company.Address.Street);

    public MotelResponse CreateErrorMotelResponse(MotelRequestDTO request, string status)
        => new MotelResponse(
            DateTime.ParseExact(request.CheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            request.RoomType,
            0,
            DateTime.ParseExact(request.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            TimeSpan.ParseExact(request.Hour, @"hh\:mm", CultureInfo.InvariantCulture),
            request.CustomerEmail,
            request.CompanyName,
            request.Address,
            status
            );
    
   
    public MotelResponse CreateSuccessMotelResponse(MotelRequestDTO request, AppointmentResponse appointment)
       => new MotelResponse(
            DateTime.ParseExact(request.CheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            request.RoomType,
            appointment.IdentificationAppointment,
            appointment.Date,
            appointment.Hour,
            appointment.CustomerName,
            appointment.CompanyName,
            appointment.Address,
            appointment.Status);
}

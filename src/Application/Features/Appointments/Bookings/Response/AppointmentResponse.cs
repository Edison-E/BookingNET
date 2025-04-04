namespace BookPro.Application.Features.Appointments.Bookings.Response;

public class AppointmentResponse : AppointmentResponseBase
{

    public AppointmentResponse(
        string status,
        int id,
        DateTime date,
        TimeSpan hour,
        string customerName,
        string companyName,
        string address) : base(id, date, hour, customerName, companyName, address, status)
    {
    }
}

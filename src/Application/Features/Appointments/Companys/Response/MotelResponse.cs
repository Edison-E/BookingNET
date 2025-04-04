using BookPro.Application.Features.Appointments.Bookings.Response;

namespace BookPro.Application.Features.Appointments.Companys.Response;

public class MotelResponse : AppointmentResponseBase
{
    public DateTime Checkout { get; set; }
    public string Type { get; set; }
    public MotelResponse(
        DateTime checkout,
        string type,
        int id,
        DateTime date,
        TimeSpan hour,
        string customerName,
        string companyName,
        string address,
        string status) : base(id, date, hour, customerName, companyName, address, status)
    {
        Checkout = checkout;
        Type = type;
    }
}

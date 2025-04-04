namespace BookPro.Application.Features.Appointments.Bookings.DTOs;

public class AppointmentBaseRequest
{
    public string CustomerEmail { get; set; }
    public string CompanyName { get; set; }
    public string Date { get; set; }
    public string Hour { get; set; }
    public string Address { get; set; }
}

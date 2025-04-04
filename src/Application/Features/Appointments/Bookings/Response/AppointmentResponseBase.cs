namespace BookPro.Application.Features.Appointments.Bookings.Response;

public abstract class AppointmentResponseBase
{
    public int IdentificationAppointment { get; set; }
    public string CustomerName { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Hour { get; set; }


    public AppointmentResponseBase(
        int id, 
        DateTime date, 
        TimeSpan hour, 
        string customerName,
        string companyName,
        string address,
        string status)
    {
        IdentificationAppointment = id;
        Date = date;
        Hour = hour;
        CustomerName = customerName;
        CompanyName = companyName;
        Address = address;
        Status = status;
    }
}
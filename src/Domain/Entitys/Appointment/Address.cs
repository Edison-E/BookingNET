namespace BookPro.Domain.Entitys.Appointment;

public class Address
{
    public int Id { get; set; }
    public int PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Street { get; set; }
}

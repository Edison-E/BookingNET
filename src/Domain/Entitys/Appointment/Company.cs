using System.ComponentModel.DataAnnotations.Schema;

namespace BookPro.Domain.Entitys.Appointment;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PhoneNumber {  get; set; }

    public int AddressId { get; set; }
    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    public int ServiceId { get; set; }
    [ForeignKey("ServiceId")]
    public TypeServices TypeService { get; set; }
    
}

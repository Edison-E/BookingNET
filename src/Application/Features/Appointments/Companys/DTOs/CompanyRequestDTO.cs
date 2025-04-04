namespace BookPro.Application.Features.Appointments.Companys.DTOs;

public class CompanyRequestDTO
{
    public string Name { get; set; }
    public int PhoneNumber { get; set; }
    public string TypeService { get; set; }
    public AddressDTO Address { get; set; }
    
}

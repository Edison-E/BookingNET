namespace BookPro.Application.Features.Account.DTO;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string DateBirth { get; set; }
    public int PhoneNumber { get; set; }
}

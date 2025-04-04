
namespace BookPro.Application.Features.Authentication.Account.DTOs;

public class ResetPasswordRequest
{
    public string NewPassword {  get; set; }
    public string Token { get; set; }
}

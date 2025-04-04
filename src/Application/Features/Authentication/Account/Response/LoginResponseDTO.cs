namespace BookPro.Application.Features.Authentication.Account.Response;

public class LoginResponseDTO : ResponseDTO
{
    public string Token { get; set; }

    public LoginResponseDTO(List<string> message, string token, bool success)
        : base(success, message)
    {
        Token = token;
    }

    public override string GetMessage()
    {
        return string.Join(" ", Message);
    }
}

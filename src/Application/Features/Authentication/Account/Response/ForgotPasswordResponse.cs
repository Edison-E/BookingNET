namespace BookPro.Application.Features.Authentication.Account.Response;

public class ForgotPasswordResponse
{
    public string Token { get; set; }
    public bool Success {  get; set; }

    public ForgotPasswordResponse ( string token, bool success)
    {
        Token = token;
        Success = success;
    }
}

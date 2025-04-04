namespace BookPro.Application.Features.Token.Interface;

public interface ITokenGenerateFactory
{
    public string GenerateAccessToken(string email);
    public string GenerateRefreshToken();
    public string GenerateResetToken(string email);
}

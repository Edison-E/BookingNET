namespace BookPro.Application.Features.Token.Factory;
public static class TokenResponseFactory
{
    public static TokenResponseDTO CreateTokenResponse(bool success, List<string> message, string tokenAccess) =>
        new TokenResponseDTO(success, message, tokenAccess);
}


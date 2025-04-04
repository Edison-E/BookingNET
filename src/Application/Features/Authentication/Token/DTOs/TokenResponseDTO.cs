namespace BookPro.Application.Features.Token.DTOs;

public class TokenResponseDTO : ResponseDTO
{
    public string TokenAccess { get; set; }
    public TokenResponseDTO(bool success, List<string> message, string tokenAccess) : base(success, message)
    {
        TokenAccess = tokenAccess;
    }

    public override string GetMessage()
    {
        throw new NotImplementedException();
    }
}

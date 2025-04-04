namespace BookPro.Application.Features.Authentication.Account.Response;

public class RegisterResponseDTO : ResponseDTO
{

    public RegisterResponseDTO(List<string> message, bool success) : base(success, message)
    {
    }

    public override string GetMessage()
    {
        return string.Join(" ", Message);
    }
}

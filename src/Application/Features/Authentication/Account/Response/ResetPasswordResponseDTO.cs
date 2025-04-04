
namespace BookPro.Application.Features.Authentication.Account.Response;

public class ResetPasswordResponseDTO : ResponseDTO
{

    public ResetPasswordResponseDTO(bool success, List<string> message) : base(success, message)
    {
    }

    public override string GetMessage()
    {
        throw new NotImplementedException();
    }
}

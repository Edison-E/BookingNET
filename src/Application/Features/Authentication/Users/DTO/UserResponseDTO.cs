namespace BookPro.Application.Features.Authentication.Users.DTO;

public class UserResponseDTO : ResponseDTO
{
    public UserDTO UserDTO { get; set; }
    public UserResponseDTO(bool success, List<string> message, UserDTO userDTO) : base(success, message)
    {
        UserDTO = userDTO;
    }

    public override string GetMessage()
    {
        return string.Join(", ", Message);
    }
}

namespace BookPro.Common.Responses;

public class ErrorResponseDTO : ResponseDTO
{
    public string State { get; set; }
    public ErrorResponseDTO(bool success, List<string> message, string state) : base(success, message)
    {
        State = state;
    }

    public override string GetMessage()
    {
        return string.Join(" ", Message);
    }
}

namespace BookPro.Common.Responses;

public abstract class ResponseDTO
{
    public bool Success { get; set; }
    public List<string> Message { get; set; }

    public ResponseDTO(bool success, List<string> message)
    {
        Success = success;
        Message = message;
    }
    public abstract string GetMessage();
}

namespace BookPro.Common.Responses;

public class SuccessResponseDTO<T> : ResponseDTO
{
    public T Data { get; set; }
    public SuccessResponseDTO(bool success, List<string> message, T data) : base(success, message)
    {
        Data = data;
    }
    public override string GetMessage()
    {
        return string.Join(" ", Message);
    }
}

namespace BookPro.Common.Responses;

public static class ResponseDTOFactory
{
    public static SuccessResponseDTO<T> CreateSuccess<T>(List<string> message, T data) => new SuccessResponseDTO<T>(true, message, data);
    public static ErrorResponseDTO CreateError(List<string> message, string state) => new ErrorResponseDTO(false, message, state);
}

namespace BookPro.Application.Features.Account.Factory;

public static class AccountResponseFactory
{
    public static LoginResponseDTO CreateLoginResponse(List<string> message, string token, bool success) =>
        new LoginResponseDTO(message, token, success);
    public static RegisterResponseDTO CreateRegisterResponse(List<string> message, bool success) =>
        new RegisterResponseDTO(message, success);

    public static ResetPasswordResponseDTO CreateResetPasswordResponseDTO(List<string> message, bool success) =>
        new ResetPasswordResponseDTO(success, message);

    public static ForgotPasswordResponse CreateResetForgotPassword(string token, bool success) =>
        new ForgotPasswordResponse(token, success);
}

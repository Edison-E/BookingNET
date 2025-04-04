using BookPro.Application.Features.Authentication.Account.DTOs;

namespace BookPro.Application.Features.Account.Interface;

public interface IAccountService
{
    Task<LoginResponseDTO> Login(LoginRequest login);
    Task<RegisterResponseDTO> Register(RegisterRequest register);
    Task<ResetPasswordResponseDTO> ResetPassword(ResetPasswordRequest resetPassword);
    Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);
}

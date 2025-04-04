using BookPro.Application.Features.Authentication.Account.DTOs;

namespace BookPro.Api.Features.Authentication.Account;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("session")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        LoginResponseDTO response = await _accountService.Login(login);

        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest register)
    {

        RegisterResponseDTO response = await _accountService.Register(register);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);

    }

    [HttpPost("password-recovery")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await _accountService.ForgotPassword(request);

        if( !response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("password-change")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
    {

        ResetPasswordResponseDTO response = await _accountService.ResetPassword(resetPassword);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);

    }
}

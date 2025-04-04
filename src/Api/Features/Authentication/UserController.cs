namespace BookPro.Api.Features.Authentication;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("profiles")]
    public async Task<IActionResult> GetProfile()
    {
        string email = User.FindFirst(ClaimTypes.Email).Value;

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is empty");
        }

        UserResponseDTO response = await _userService.GetUser(email);

        if (!response.Success)
        {
            return Unauthorized("Not have unauthorized !!!");
        }

        return Ok(response);
    }
}

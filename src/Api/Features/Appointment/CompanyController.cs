using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Interface;

namespace BookPro.Api.Features.Appointment;

[Route("api/companies")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController (ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("companies")]
    public async Task<IActionResult> GetCompanies()
        => Ok( await _companyService.Services());

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms([FromQuery] RoomRequestDTO request)
        => Ok(await _companyService.Rooms(request));

    [HttpPost("company")]
    public async Task<IActionResult> AddCompany([FromBody] CompanyRequestDTO request)
    {
        var response = await _companyService.AddCompany(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    
}

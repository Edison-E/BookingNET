using BookPro.Application.Features.Appointments.Companys.DTOs;

namespace BookPro.Api.Features.Appointment;

[Route("api/appointments")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public AppointmentController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("appointment")]
    public async Task<IActionResult> Appointment([FromBody] AppointmentRequestDTO appointment)
        => Ok(await _bookingService.Appointment(appointment));

    [HttpPost("appointment-motel")]
    public async Task<IActionResult> AppointmentMotel([FromBody] MotelRequestDTO appointment)
        => Ok(await _bookingService.Appointment(appointment));
}

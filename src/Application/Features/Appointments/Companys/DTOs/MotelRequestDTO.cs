namespace BookPro.Application.Features.Appointments.Companys.DTOs;

public class MotelRequestDTO : AppointmentBaseRequest
{
    public string RoomType { get; set; }
    public string CheckOut { get; set; }
}

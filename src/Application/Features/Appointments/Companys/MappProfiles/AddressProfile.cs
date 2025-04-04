using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.MappProfiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<AddressDTO, Address>();
    }
}

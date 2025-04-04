using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.MappProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile ()
    {
        CreateMap<CompanyRequestDTO, Company>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.TypeService, opt => opt.Ignore())
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceId, opt => opt.Ignore());
    }
}

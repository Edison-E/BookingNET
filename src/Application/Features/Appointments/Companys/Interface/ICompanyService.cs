using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyService
{
    Task<CompanyServiceResponse<CompaniesResponse>> Services();
    Task<CompanyServiceResponse<RoomsResponse>> Rooms(RoomRequestDTO request);
    //Task<CompanyResponse> AddCompany(CompanyRequestDTO request);
    Task<CompanyResponse> AddCompany(CompanyRequestDTO request);
}

using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Application.Features.Appointments.Companys.Response;

namespace BookPro.Application.Features.Appointments.Companys.Interface;

public interface ICompanyResponseFactory
{
    CompanyServiceResponse<CompaniesResponse> CreateSuccesCompany(string message, List<CompaniesResponse> list);
    CompanyServiceResponse<RoomsResponse> CreateSuccesRoom(string message, List<RoomsResponse> list);

    CompanyServiceResponse<CompaniesResponse> CreateErrorCompany(string message);
    CompanyServiceResponse<RoomsResponse> CreateErrorRoom(string message);

    CompanyResponse CreateCompanyResponse(List<string> message, bool sucess);

    AddressResponse CreateAddressResponse(List<string> message, bool sucess);
}

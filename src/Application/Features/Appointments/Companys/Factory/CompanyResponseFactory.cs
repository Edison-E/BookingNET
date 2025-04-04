using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Application.Features.Appointments.Companys.Response;

namespace BookPro.Application.Features.Appointments.Companys.Factory;

public class CompanyResponseFactory : ICompanyResponseFactory
{
    public CompanyServiceResponse<CompaniesResponse> CreateErrorCompany(string message)
       => new CompanyServiceResponse<CompaniesResponse>(message, null);

    public CompanyServiceResponse<RoomsResponse> CreateErrorRoom(string message)
       => new CompanyServiceResponse<RoomsResponse>(message, null);

    public CompanyServiceResponse<CompaniesResponse> CreateSuccesCompany(string message, List<CompaniesResponse> list)
       => new CompanyServiceResponse<CompaniesResponse>(message, list);

    public CompanyServiceResponse<RoomsResponse> CreateSuccesRoom(string message, List<RoomsResponse> list)
       => new CompanyServiceResponse<RoomsResponse>(message, list);

    public AddressResponse CreateAddressResponse(List<string> message, bool sucess)
        => new AddressResponse { Messages = message, Success = sucess };

    public CompanyResponse CreateCompanyResponse(List<string> message, bool sucess)
       => new CompanyResponse { Message = message, Success = sucess };
}
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Factory;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Domain.Entitys.Appointment;
using BookPro.Domain.Entitys.Appointment.log;
using BookPro.Domain.Entitys.Appointment.Resource;

namespace BookPro.Application.Features.Appointments.Companys;

public class CompanyService : ServiceBase, ICompanyService
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMotelRepository _motelRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IServicesRepository _servicesRepository;
    private readonly ILogMessageCompany _logHelper;
    private readonly ICompanyResponseFactory _companyResponseFactory;
    private readonly ICompanyValidator _companyValidator;

    public CompanyService(
        IMapper mapper, 
        ILogger<CompanyService> logger, 
        IManagerResourceLenguaje managerLenguaje,
        ICompaniesRepository companiesRepository,
        IRoomRepository roomRepository,
        IMotelRepository motelRepository,
        ILogMessageCompany logHelper,
        ICompanyResponseFactory companyResponseFactory,
        IAddressRepository addressRepository,
        IServicesRepository servicesRepository,
        ICompanyValidator companyValidator) 
        : base(mapper, logger, managerLenguaje)
    {
        _companiesRepository = companiesRepository;
        _roomRepository = roomRepository;
        _motelRepository = motelRepository;
        _logHelper = logHelper;
        _companyResponseFactory = companyResponseFactory;
        _addressRepository = addressRepository;
        _servicesRepository = servicesRepository;
        _companyValidator = companyValidator;
    }

    public async Task<CompanyServiceResponse<CompaniesResponse>> Services()
    {
        try
        {
            var list = new List<CompaniesResponse>();
            var companies = await _companiesRepository.GetAllCompanies();

            foreach (var item in companies)
            {
                var service = new CompaniesResponse
                {
                    CompanyName = item.Name,
                    Address = item.Address.Street
                };

                list.Add(service);
            }

            return _companyResponseFactory.CreateSuccesCompany("Companies have been generated correctly.", list);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerCompany.ErrorGetCompanies), ex);
            return _companyResponseFactory.CreateErrorCompany("Could not load the companies, please try again later.");
        }
    }

    public async Task<CompanyServiceResponse<RoomsResponse>> Rooms(RoomRequestDTO request)
    {
        try
        {
            var list = new List<RoomsResponse>();

            var company = await _companiesRepository.GetCompaniesByNameAndAddress(request.CompanyName, request.Address);

            if (company == null)
            {
                return _companyResponseFactory.CreateSuccesRoom("Could not load the rooms of this company, please check the name and address.", list);

            }

            var motel = await _motelRepository.GetByCompany(company.Id);
            var rooms = await _roomRepository.GetAllRoomByMotel(motel.Id);

           

            foreach (var item in rooms)
            {
                var service = new RoomsResponse
                {
                    Room = item.RoomType,
                    Available = item.Available,
                };

                list.Add(service);
            }

            return _companyResponseFactory.CreateSuccesRoom("The rooms of this company have been generated correctly.", list);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerCompany.ErrorGetRooms), ex);
            return _companyResponseFactory.CreateErrorRoom("Could not load the rooms of this company, please try again later.");
        }
    }

    public async Task<CompanyResponse> AddCompany(CompanyRequestDTO request)
    {
        try
        {
            var requestIsvalid = _companyValidator.ValidationCompanyRequest(request);
            if (!requestIsvalid.Valid)
            {
                LogWarning(_logHelper.GetMessage(LoggerCompany.RequestCompanyInvalid) + requestIsvalid.GetMessageAsString());
                return _companyResponseFactory.CreateCompanyResponse(requestIsvalid.Message, false);
            }

            var addressExists = await _addressRepository.GetAddressbyStreet(request.Address.Street);
            if (addressExists != null)
            {
                LogWarning(_logHelper.GetMessage(LoggerCompany.AddressExist));
                return _companyResponseFactory.CreateCompanyResponse(new List<string> { _managerLenguaje.GetMessage(ErrorCompany.AddressExist) }, false);
            }

            var companie = _mapper.Map<Company>(request);
            var address = _mapper.Map<Address>(request.Address);

            var isAddCompany = await _companiesRepository.AddCompany(companie, address, request.TypeService);
            if (!isAddCompany)
            {
                LogWarning(_logHelper.GetMessage(LoggerCompany.CompanyNotInsert));
                return _companyResponseFactory.CreateCompanyResponse(new List<string> { _managerLenguaje.GetMessage(ErrorCompany.ErrorInsertCompany) }, false);
            }

            return _companyResponseFactory.CreateCompanyResponse(new List<string> { _managerLenguaje.GetMessage(SuccessCompany.SuccessInsertCompany) }, true);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerCompany.ErrorProcessInsertCompany), ex);
            return _companyResponseFactory.CreateCompanyResponse(new List<string> { _managerLenguaje.GetMessage(ErrorCompany.ErrorProcessCompany) }, false);
        }
    }

    
}

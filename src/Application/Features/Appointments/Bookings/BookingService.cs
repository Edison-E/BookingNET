using BookPro.Application.Features.Appointments.Bookings.Response;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Domain.Entitys.Appointment;
using BookPro.Domain.Entitys.Appointment.log;
using BookPro.Domain.Entitys.Appointment.Resource;
using BookPro.Domain.Entitys.Users.logger;
using System.Globalization;

namespace BookPro.Application.Features.Appointments.Bookings;

public class BookingService : ServiceBase, IBookingService
{
    private readonly IUserRepository _userRepository;
    private readonly IAppointmentResponseFactory _appointmentResponseFactory;
    private readonly ILogMessageBooking _logHelper;
    private readonly ILogMessageUser _logHelperUser;
    private readonly IUserValidator _userValidator;
    private readonly IAppointmentRequestValidator _requestValidator;

    private readonly ICompaniesRepository _companiesRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMotelRepository _motelRepository;

    public BookingService(
        IMapper mapper,
        ILogger<BookingService> logger,
        IManagerResourceLenguaje managerLenguaje,
        IUserRepository userRepository,
        IAppointmentResponseFactory appointmentResponseFactory,
        ILogMessageBooking logHelper,
        IUserValidator userValidator,
        ILogMessageUser logMessageUser,
        ICompaniesRepository companiesRepository,
        IAppointmentRepository appointmentRepository,
        IRoomRepository roomRepository,
        IMotelRepository motelRepository,
        IAppointmentRequestValidator requestValidator

        ) :
        base(mapper, logger, managerLenguaje)
    {
        _userRepository = userRepository;
        _appointmentResponseFactory = appointmentResponseFactory;
        _logHelper = logHelper;
        _logHelperUser = logMessageUser;
        _userValidator = userValidator;
        _requestValidator = requestValidator;

        _companiesRepository = companiesRepository;
        _appointmentRepository = appointmentRepository;
        _roomRepository = roomRepository;
        _motelRepository = motelRepository;
    }

    public async Task<AppointmentResponseBase> Appointment(AppointmentBaseRequest request) => 
        await RedirectToService(request);

    private async Task<AppointmentResponseBase> RedirectToService(AppointmentBaseRequest request)
    {
        var requestIsValid = _requestValidator.ValidateRequest(request);

        if (!requestIsValid.Valid)
        {
            return _appointmentResponseFactory.CreateErrorAppointmentResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorParametersRequest));
        }

        return request switch
        {
            MotelRequestDTO motelRequest => await MotelService(motelRequest),
            AppointmentRequestDTO appointmentRequest => await AppointmentService(appointmentRequest),
            _ => _appointmentResponseFactory.CreateErrorAppointmentResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorServiceNotExist))
        };
    }

    private async Task<AppointmentResponse> AppointmentService(AppointmentRequestDTO request) => 
        await StandardFlowAppointment(request);

    private async Task<MotelResponse> MotelService(MotelRequestDTO request)
    {
        try
        {
            var response = await StandardFlowAppointment(request);

            var company = await _companiesRepository.GetCompaniesByNameAndAddress(request.CompanyName, request.Address);
            var motel = await _motelRepository.GetByCompany(company.Id);
            var room = await _roomRepository.GetRoomByMotel(motel.Id);

            room.CheckOut = DateTime.ParseExact(request.CheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            room.Available = false;

            var isUpdateRoom = await _roomRepository.UpdateRoom(room.Id, room);

            if (!isUpdateRoom)
            {
                LogWarning(_logHelper.GetMessage(LoggerBooking.ErrorUpdateRoom));
                return _appointmentResponseFactory.CreateErrorMotelResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorUpdateRoom));
            }

            return _appointmentResponseFactory.CreateSuccessMotelResponse(request, response);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerBooking.ErrorProcessingAppointment),ex);
            return _appointmentResponseFactory.CreateErrorMotelResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorProcessingAppointment));
        }
        
    }
    
    private async Task<AppointmentResponse> StandardFlowAppointment(AppointmentBaseRequest request)
    {
        try
        {
            var user = await _userRepository.GetByEmail(request.CustomerEmail);

            var userIsValid = _userValidator.ValidateUser(user);

            if (!userIsValid.Valid)
            {
                LogWarning(_logHelperUser.GetMessage(LoggerUser.NotFindUser));
                return _appointmentResponseFactory.CreateErrorAppointmentResponse(request, _managerLenguaje.GetMessage(ErrorUser.UserNotFind));
            }

            var company = await _companiesRepository.GetCompaniesByNameAndAddress(request.CompanyName, request.Address);

            var appointment = new Appointment
            {
                Date = DateTime.ParseExact(request.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Hour = TimeSpan.ParseExact(request.Hour, @"hh\:mm", CultureInfo.InvariantCulture),
                Status = "Confirmed",
                CustomerId = user.Id,
                CompanyId = company.Id,
                Created = DateTime.UtcNow,
            };

            var isSavedAppointemnt = await _appointmentRepository.InsertAppointment(appointment);

            if (!isSavedAppointemnt)
            {
                LogWarning(_logHelper.GetMessage(LoggerBooking.ErrorSaveAppointment));
                return _appointmentResponseFactory.CreateErrorAppointmentResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorSaveAppointment));
            }

            var appointmentLast = await _appointmentRepository.GetLastAppointmentByUser(user.Id);

            return _appointmentResponseFactory.CreateSuccessAppointmentResponse(appointmentLast);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerBooking.ErrorProcessingAppointment), ex);
            return _appointmentResponseFactory.CreateErrorAppointmentResponse(request, _managerLenguaje.GetMessage(ErrorAppointment.ErrorProcessingAppointment));
        }
       
    }
}
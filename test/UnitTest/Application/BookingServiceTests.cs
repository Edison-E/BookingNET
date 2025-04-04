using AutoMapper;
using BookPro.Application.Features.Appointments.Bookings;
using BookPro.Application.Features.Appointments.Bookings.DTOs;
using BookPro.Application.Features.Appointments.Bookings.Interface;
using BookPro.Application.Features.Appointments.Bookings.Response;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Application.Features.Authentication.Users.Interface;
using BookPro.Application.Validation;
using BookPro.Common.Localization;
using BookPro.Common.Logger.interfaces;
using BookPro.Domain.Entitys.Appointment;
using BookPro.Domain.Entitys.Appointment.Resource;
using BookPro.Domain.interfaces.Appointment;
using BookPro.Domain.interfaces.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;

namespace BookPro.UnitTest.Application;
public class BookingServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAppointmentResponseFactory> _appointmentResponseFactoryMock;
    private readonly Mock<ILogMessageBooking> _logHelperMock;
    private readonly Mock<ILogMessageUser> _logHelperUserMock;
    private readonly Mock<IUserValidator> _userValidatorMock;
    private readonly Mock<IAppointmentRequestValidator> _requestValidatorMock;
    private readonly Mock<ICompaniesRepository> _companiesRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMotelRepository> _motelRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<BookingService>> _loggerMock;
    private readonly Mock<IManagerResourceLenguaje> _managerLenguajeMock;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _appointmentResponseFactoryMock = new Mock<IAppointmentResponseFactory>();
        _logHelperMock = new Mock<ILogMessageBooking>();
        _logHelperUserMock = new Mock<ILogMessageUser>();
        _userValidatorMock = new Mock<IUserValidator>();
        _requestValidatorMock = new Mock<IAppointmentRequestValidator>();
        _companiesRepositoryMock = new Mock<ICompaniesRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _motelRepositoryMock = new Mock<IMotelRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<BookingService>>();
        _managerLenguajeMock = new Mock<IManagerResourceLenguaje>();

        _bookingService = new BookingService(
            _mapperMock.Object,
            _loggerMock.Object,
            _managerLenguajeMock.Object,
            _userRepositoryMock.Object,
            _appointmentResponseFactoryMock.Object,
            _logHelperMock.Object,
            _userValidatorMock.Object,
            _logHelperUserMock.Object,
            _companiesRepositoryMock.Object,
            _appointmentRepositoryMock.Object,
            _roomRepositoryMock.Object,
            _motelRepositoryMock.Object,
            _requestValidatorMock.Object
        );
    }

    [Fact]
    public async Task Appointment_ValidAppointmentRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new AppointmentRequestDTO { 
            CustomerEmail = "test@example.com", 
            CompanyName = "Test Company",
            Address = "Test Address", 
            Date = "01/01/2002",
            Hour = "09:30"
        };
        var validationResult = new ValidationResult { Message = new List<string>() };
        var user = new User { Id = 1, Email = "test@example.com" };
        var company = new Company { Id = 1, Name = "Test Company" };
        var appointment = new Appointment { Id = 1, CustomerId = 1, CompanyId = 1, Date = DateTime.Now, Hour = TimeSpan.FromHours(1), Status = "Confirmed" };
        var appointmentResponse = new AppointmentResponse("Confirmed", 1, DateTime.Now, TimeSpan.FromHours(1), "Test User", "Test Company", "Test Address");

        _requestValidatorMock.Setup(v => v.ValidateRequest(request)).Returns(validationResult);
        _userRepositoryMock.Setup(r => r.GetByEmail(request.CustomerEmail)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(validationResult);
        _companiesRepositoryMock.Setup(r => r.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ReturnsAsync(company);
        _appointmentRepositoryMock.Setup(r => r.InsertAppointment(It.IsAny<Appointment>())).ReturnsAsync(true);
        _appointmentRepositoryMock.Setup(r => r.GetLastAppointmentByUser(user.Id)).ReturnsAsync(appointment);
        _appointmentResponseFactoryMock.Setup(f => f.CreateSuccessAppointmentResponse(appointment)).Returns(appointmentResponse);

        // Act
        var result = await _bookingService.Appointment(request);

        // Assert
        _appointmentResponseFactoryMock.Verify(f => f.CreateSuccessAppointmentResponse(appointment), Times.Once);
    }

    [Fact]
    public async Task Appointment_InvalidRequest_ReturnsErrorResponse()
    {
        // Arrange
        var request = new AppointmentRequestDTO();
        var validationResult = new ValidationResult { Message = new List<string>() { "Invalid request" } };

        _requestValidatorMock.Setup(v => v.ValidateRequest(request)).Returns(validationResult);
        _managerLenguajeMock.Setup(m => m.GetMessage(It.IsAny<ErrorAppointment>())).Returns("Invalid request");

        // Act
        var result = await _bookingService.Appointment(request);

        // Assert
        _appointmentResponseFactoryMock.Verify(f => f.CreateErrorAppointmentResponse(request, "Invalid request"), Times.Once);
    }

    [Fact]
    public async Task Appointment_ValidMotelRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new MotelRequestDTO { CustomerEmail = "test@example.com", CompanyName = "Test Company", Address = "Test Address", CheckOut = "01/01/2023" };
        var validationResult = new ValidationResult { Message = new List<string>() };
        var user = new User { Id = 1, Email = "test@example.com" };
        var company = new Company { Id = 1, Name = "Test Company" };
        var motel = new Motel { Id = 1, CompanyId = 1 };
        var room = new Room { Id = 1, MotelId = 1, Available = true };
        var appointmentResponse = new AppointmentResponse("Confirmed", 1, DateTime.Now, TimeSpan.FromHours(1), "Test User", "Test Company", "Test Address");

        _requestValidatorMock.Setup(v => v.ValidateRequest(request)).Returns(validationResult);
        _userRepositoryMock.Setup(r => r.GetByEmail(request.CustomerEmail)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(validationResult);
        _companiesRepositoryMock.Setup(r => r.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ReturnsAsync(company);
        _motelRepositoryMock.Setup(r => r.GetByCompany(company.Id)).ReturnsAsync(motel);
        _roomRepositoryMock.Setup(r => r.GetRoomByMotel(motel.Id)).ReturnsAsync(room);
        _roomRepositoryMock.Setup(r => r.UpdateRoom(room.Id, It.IsAny<Room>())).ReturnsAsync(true);
        _appointmentResponseFactoryMock.Setup(f => f.CreateSuccessMotelResponse(request, appointmentResponse)).Returns(new MotelResponse(DateTime.Now, "Test Room", 1, DateTime.Now, TimeSpan.FromHours(1), "Test User", "Test Company", "Test Address", "Confirmed"));

        // Act
        var result = await _bookingService.Appointment(request);

        // Assert
        _appointmentResponseFactoryMock.Verify(f => f.CreateSuccessMotelResponse(request, It.IsAny<AppointmentResponse>()), Times.Once);
    }

    [Fact]
    public async Task Appointment_InvalidMotelRequest_ReturnsErrorResponse()
    {
        // Arrange
        var request = new MotelRequestDTO();
        var validationResult = new ValidationResult { Message = new List<string>() { "Invalid motel request" } };

        _requestValidatorMock.Setup(v => v.ValidateRequest(request)).Returns(validationResult);
        _managerLenguajeMock.Setup(m => m.GetMessage(It.IsAny<ErrorAppointment>())).Returns("Invalid motel request");

        // Act
        var result = await _bookingService.Appointment(request);

        // Assert
        _appointmentResponseFactoryMock.Verify(f => f.CreateErrorAppointmentResponse(request, "Invalid motel request"), Times.Once);
    }

    [Fact]
    public async Task Appointment_CompanyNull_ReturnsErrorResponse()
    {

        // Arrange
        var request = new AppointmentRequestDTO { CustomerEmail = "test@example.com", CompanyName = "CompanyNotExist", Address = "AddressNotExits", Date = "01/01/2002", Hour = "09:30"};
        var validationResult = new ValidationResult { Message = new List<string>() };
        var user = new User { Id = 1, Email = "test@example.com" };
        var company = new Company ();

        _requestValidatorMock.Setup(v => v.ValidateRequest(request)).Returns(validationResult);
        _userRepositoryMock.Setup(r => r.GetByEmail(request.CustomerEmail)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(validationResult);
        _companiesRepositoryMock.Setup(r => r.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ReturnsAsync(company);
        _managerLenguajeMock.Setup(m => m.GetMessage(It.IsAny<ErrorAppointment>())).Returns("The company not exist");
        _appointmentResponseFactoryMock.Setup(f => f.CreateErrorAppointmentResponse(request, "The company not exist")).Returns(new AppointmentResponse(
            "Fail", 0, DateTime.UtcNow, TimeSpan.ParseExact(request.Hour, @"hh\:mm", CultureInfo.InvariantCulture),  user.Email, request.CompanyName, request.Address));

        // Act
        var result = await _bookingService.Appointment(request);

        // Assert
        _appointmentResponseFactoryMock.Verify(f => f.CreateErrorAppointmentResponse(request, "The company not exist"), Times.Once);
    }
}

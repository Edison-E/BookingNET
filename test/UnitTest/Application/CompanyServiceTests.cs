using AutoMapper;
using BookPro.Application.Features.Appointments.Companys;
using BookPro.Application.Features.Appointments.Companys.DTOs;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Application.Features.Appointments.Companys.Response;
using BookPro.Common.Localization;
using BookPro.Common.Logger.interfaces;
using BookPro.Domain.Entitys.Appointment;
using BookPro.Domain.interfaces.Appointment;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookPro.UnitTest.Application;

public class CompanyServiceTests
{
    private readonly Mock<ICompaniesRepository> _companiesRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMotelRepository> _motelRepositoryMock;
    private readonly Mock<ILogMessageCompany> _logHelperMock;
    private readonly Mock<ICompanyResponseFactory> _companyResponseFactoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CompanyService>> _loggerMock;
    private readonly Mock<IManagerResourceLenguaje> _managerLenguajeMock;
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IServicesRepository> _serviceRepositoryMock;
    private readonly Mock<ICompanyValidator> _companyValidator;
    private readonly CompanyService _companyService;

    public CompanyServiceTests()
    {
        _companiesRepositoryMock = new Mock<ICompaniesRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _motelRepositoryMock = new Mock<IMotelRepository>();
        _logHelperMock = new Mock<ILogMessageCompany>();
        _companyResponseFactoryMock = new Mock<ICompanyResponseFactory>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CompanyService>>();
        _managerLenguajeMock = new Mock<IManagerResourceLenguaje>();
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _serviceRepositoryMock = new Mock<IServicesRepository>();
        _companyValidator = new Mock<ICompanyValidator>();

        _companyService = new CompanyService(
            _mapperMock.Object,
            _loggerMock.Object,
            _managerLenguajeMock.Object,
            _companiesRepositoryMock.Object,
            _roomRepositoryMock.Object,
            _motelRepositoryMock.Object,
            _logHelperMock.Object,
            _companyResponseFactoryMock.Object,
            _addressRepositoryMock.Object,
            _serviceRepositoryMock.Object,
            _companyValidator.Object
        );
    }

    [Fact]
    public async Task Services_ShouldReturnSuccessResponse_WhenCompaniesExist()
    {
        // Arrange
        var companies = new List<Company>
        {
            new Company { Name = "Company1", Address = new Address { Street = "Street1" } },
            new Company { Name = "Company2", Address = new Address { Street = "Street2" } }
        };

        var expectedResponse = new CompanyServiceResponse<CompaniesResponse>("Companies have been generated correctly.", new List<CompaniesResponse>
        {
            new CompaniesResponse { CompanyName = "Company1", Address = "Street1" },
            new CompaniesResponse { CompanyName = "Company2", Address = "Street2" }
        });

        _companiesRepositoryMock.Setup(repo => repo.GetAllCompanies()).ReturnsAsync(companies);
        _companyResponseFactoryMock.Setup(factory => factory.CreateSuccesCompany(It.IsAny<string>(), It.IsAny<List<CompaniesResponse>>()))
            .Returns(expectedResponse);

        // Act
        var result = await _companyService.Services();

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task Services_ShouldReturnErrorResponse_WhenExceptionIsThrown()
    {
        // Arrange
        var expectedResponse = new CompanyServiceResponse<CompaniesResponse>("Could not load the companies, please try again later.", new List<CompaniesResponse>());

        _companiesRepositoryMock.Setup(repo => repo.GetAllCompanies()).ThrowsAsync(new Exception());
        _companyResponseFactoryMock.Setup(factory => factory.CreateErrorCompany(It.IsAny<string>()))
            .Returns(expectedResponse);

        // Act
        var result = await _companyService.Services();

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task Rooms_ShouldReturnSuccessResponse_WhenRoomsExist()
    {
        // Arrange
        var request = new RoomRequestDTO { CompanyName = "Company1", Address = "Street1" };
        var company = new Company { Id = 1, Name = "Company1", Address = new Address { Street = "Street1" } };
        var motel = new Motel { Id = 1, CompanyId = 1 };
        var rooms = new List<Room>
        {
            new Room { RoomType = "Room1", Available = true },
            new Room { RoomType = "Room2", Available = false }
        };

        var expectedResponse = new CompanyServiceResponse<RoomsResponse>("The rooms of this company have been generated correctly.", new List<RoomsResponse>
        {
            new RoomsResponse { Room = "Room1", Available = true },
            new RoomsResponse { Room = "Room2", Available = false }
        });

        _companiesRepositoryMock.Setup(repo => repo.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ReturnsAsync(company);
        _motelRepositoryMock.Setup(repo => repo.GetByCompany(company.Id)).ReturnsAsync(motel);
        _roomRepositoryMock.Setup(repo => repo.GetAllRoomByMotel(motel.Id)).ReturnsAsync(rooms);
        _companyResponseFactoryMock.Setup(factory => factory.CreateSuccesRoom(It.IsAny<string>(), It.IsAny<List<RoomsResponse>>()))
            .Returns(expectedResponse);

        // Act
        var result = await _companyService.Rooms(request);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task Rooms_ShouldReturnErrorResponse_WhenCompanyDoesNotExist()
    {
        // Arrange
        var request = new RoomRequestDTO { CompanyName = "Company1", Address = "Street1" };
        var expectedResponse = new CompanyServiceResponse<RoomsResponse>("Could not load the rooms of this company, please check the name and address.", new List<RoomsResponse>());

        _companiesRepositoryMock.Setup(repo => repo.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ReturnsAsync((Company)null);
        _companyResponseFactoryMock.Setup(factory => factory.CreateSuccesRoom(It.IsAny<string>(), It.IsAny<List<RoomsResponse>>()))
            .Returns(expectedResponse);

        // Act
        var result = await _companyService.Rooms(request);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task Rooms_ShouldReturnErrorResponse_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new RoomRequestDTO { CompanyName = "Company1", Address = "Street1" };
        var expectedResponse = new CompanyServiceResponse<RoomsResponse>("Could not load the rooms of this company, please try again later.", new List<RoomsResponse>());

        _companiesRepositoryMock.Setup(repo => repo.GetCompaniesByNameAndAddress(request.CompanyName, request.Address)).ThrowsAsync(new Exception());
        _companyResponseFactoryMock.Setup(factory => factory.CreateErrorRoom(It.IsAny<string>()))
            .Returns(expectedResponse);

        // Act
        var result = await _companyService.Rooms(request);

        // Assert
        Assert.Equal(expectedResponse, result);
    }
}

using AutoMapper;
using BookPro.Application.Features.Account.DTO;
using BookPro.Application.Features.Account.Interface;
using BookPro.Application.Features.Authentication.Account;
using BookPro.Application.Features.Authentication.Users.Interface;
using BookPro.Application.Features.Token.DTOs;
using BookPro.Application.Features.Token.Interface;
using BookPro.Application.Validation;
using BookPro.Common.Helper.interfaces;
using BookPro.Common.Localization;
using BookPro.Domain.interfaces.Authentication;
using BookPro.Domain.Entitys.Users.Resource;
using Microsoft.Extensions.Logging;
using Moq;
using BookPro.Common.Logger.interfaces;
using BookPro.Application.Features.Authentication.Account.Interface;

namespace BookPro.UnitTest.Application;

public class AccountServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAccountValidator> _accountValidatorMock;
    private readonly Mock<IUserValidator> _userValidatorMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogMessageAccount> _logHelperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<AccountService>> _loggerMock;
    private readonly Mock<IManagerResourceLenguaje> _managerLenguajeMock;
    private readonly Mock<ILogMessageUser> _logHelperUser;
    private readonly Mock<IEmailServices> _emailService;
    private readonly Mock<ITokenGenerateFactory> _tokenGenerateFactory;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _accountValidatorMock = new Mock<IAccountValidator>();
        _userValidatorMock = new Mock<IUserValidator>();
        _tokenServiceMock = new Mock<ITokenService>();
        _logHelperMock = new Mock<ILogMessageAccount>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<AccountService>>();
        _managerLenguajeMock = new Mock<IManagerResourceLenguaje>();
        _logHelperUser = new Mock<ILogMessageUser>();
        _emailService = new Mock<IEmailServices>();
        _tokenGenerateFactory = new Mock<ITokenGenerateFactory>();

        _accountService = new AccountService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _accountValidatorMock.Object,
            _userValidatorMock.Object,
            _tokenServiceMock.Object,
            _managerLenguajeMock.Object,
            _logHelperMock.Object,
            _logHelperUser.Object,
            _emailService.Object,
            _tokenGenerateFactory.Object
        );
    }

    [Fact]
    public async Task Login_CorrectCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var loginDto = new LoginRequest { Email = "test@example.com", Password = "password" };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password") };
        var tokenResponse = new TokenResponseDTO(true, new List<string>(), "token");

        _accountValidatorMock.Setup(v => v.ValidationLogin(loginDto)).Returns(new ValidationResult { Message = new List<string>() });
        _userRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string>() });
        _accountValidatorMock.Setup(v => v.ValidationCredentials(loginDto, user)).Returns(new ValidationResult { Message = new List<string>() });
        _tokenServiceMock.Setup(t => t.GetAccessToken(user)).ReturnsAsync(tokenResponse);

        // Act
        var result = await _accountService.Login(loginDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("token", result.Token);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnFailure()
    {
        // Arrange
        var loginDto = new LoginRequest { Email = "test@example.com", Password = "wrongpassword" };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password") };

        _accountValidatorMock.Setup(v => v.ValidationLogin(loginDto)).Returns(new ValidationResult { Message = new List<string>() });
        _userRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string>() });
        _accountValidatorMock.Setup(v => v.ValidationCredentials(loginDto, user)).Returns(new ValidationResult {  Message = new List<string> { "Invalid password" } });

        // Act
        var result = await _accountService.Login(loginDto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Invalid password", result.Message);
    }

    [Fact]
    public async Task Login_UserNotFound_ShouldReturnFailure()
    {
        // Arrange
        var loginDto = new LoginRequest { Email = "test@example.com", Password = "password" };
        User user = null;

        _accountValidatorMock.Setup(v => v.ValidationLogin(loginDto)).Returns(new ValidationResult { Message = new List<string>() });
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string> { "User not found" } });

        // Act
        var result = await _accountService.Login(loginDto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("User not found", result.Message);
    }

    [Fact]
    public async Task Register_ValidData_ShouldReturnSuccess()
    {
        // Arrange
        var registerDto = new RegisterRequest { Email = "test@example.com", Password = "password", Name = "Test User", DateBirth = "01-01-2000", PhoneNumber = 123456789 };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), Name = "Test User", DateBirth = "01-01-2000", PhoneNumber = 123456789 };

        _accountValidatorMock.Setup(v => v.ValidationRegister(registerDto)).Returns(new ValidationResult { Message = new List<string>() });
        _userRepositoryMock.Setup(r => r.GetByEmail(registerDto.Email)).ReturnsAsync((User)null);
        _mapperMock.Setup(m => m.Map<User>(registerDto)).Returns(user);
        _userRepositoryMock.Setup(r => r.Insert(user)).ReturnsAsync(true);
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessUser.UserRegister)).Returns("User registered successfully");

        // Act
        var result = await _accountService.Register(registerDto);

        // Assert
        Assert.True(result.Success);
        Assert.Contains("User registered successfully", result.Message);
    }

    [Fact]
    public async Task Register_UserAlreadyExists_ShouldReturnFailure()
    {
        // Arrange
        var registerDto = new RegisterRequest { Email = "test@example.com", Password = "password", Name = "Test User", DateBirth = "01-01-2000", PhoneNumber = 123456789 };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), Name = "Test User", DateBirth = "01-01-2000", PhoneNumber = 123456789 };

        _accountValidatorMock.Setup(v => v.ValidationRegister(registerDto)).Returns(new ValidationResult { Message = new List<string>() });
        _userRepositoryMock.Setup(r => r.GetByEmail(registerDto.Email)).ReturnsAsync(user);
        _managerLenguajeMock.Setup(m => m.GetMessage(ErrorUser.AccountExist)).Returns("Account already exists");

        // Act
        var result = await _accountService.Register(registerDto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Account already exists", result.Message);
    }

    [Fact]
    public async Task Register_InvalidData_ShouldReturnFailure()
    {
        // Arrange
        var registerDto = new RegisterRequest { Email = "test@example.com", Password = "password", Name = "Test User", DateBirth = "01-01-2000", PhoneNumber = 123456789 };

        _accountValidatorMock.Setup(v => v.ValidationRegister(registerDto)).Returns(new ValidationResult { Message = new List<string> { "Invalid data" } });

        // Act
        var result = await _accountService.Register(registerDto);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Invalid data", result.Message);
    }
}

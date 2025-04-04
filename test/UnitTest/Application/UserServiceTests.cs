using AutoMapper;
using BookPro.Application.Features.Authentication.Users;
using BookPro.Application.Features.Authentication.Users.DTO;
using BookPro.Application.Features.Authentication.Users.Interface;
using BookPro.Application.Validation;
using BookPro.Common.Localization;
using BookPro.Common.Logger.interfaces;
using BookPro.Domain.interfaces.Authentication;
using BookPro.Domain.Entitys.Users.Resource;
using Microsoft.Extensions.Logging;
using Moq;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.UnitTest.Application;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserValidator> _userValidatorMock;
    private readonly Mock<ILogMessageUser> _logHelperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IManagerResourceLenguaje> _managerLenguajeMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userValidatorMock = new Mock<IUserValidator>();
        _logHelperMock = new Mock<ILogMessageUser>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _managerLenguajeMock = new Mock<IManagerResourceLenguaje>();

        _userService = new UserService(
            _mapperMock.Object,
            _loggerMock.Object,
            _userRepositoryMock.Object,
            _userValidatorMock.Object,
            _managerLenguajeMock.Object,
            _logHelperMock.Object
        );
    }

    [Fact]
    public async Task GetUser_UserExists_ShouldReturnUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User { Email = email, Name = "Test User" };
        var userDTO = new UserDTO { Email = email, Name = "Test User" };

        _userRepositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string>() });
        _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessUser.UserFind)).Returns("User found successfully");

        // Act
        var result = await _userService.GetUser(email);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(userDTO, result.UserDTO);
        Assert.Contains("User found successfully", result.Message);
    }

    [Fact]
    public async Task GetUser_UserDoesNotExist_ShouldReturnError()
    {
        // Arrange
        var email = "test@example.com";
        User user = null;

        _userRepositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(user);
        _userValidatorMock.Setup(v => v.ValidateUser(user)).Returns(new ValidationResult { Message = new List<string> { "User invalid" } });
        _logHelperMock.Setup(l => l.GetMessage(LoggerUser.NotFindUser)).Returns("User invalid");
        _managerLenguajeMock.Setup(m => m.GetMessage(ErrorUser.UserInvalid)).Returns("User invalid");

        // Act
        var result = await _userService.GetUser(email);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.UserDTO);
        Assert.Contains("User invalid", result.Message);
    }

    [Fact]
    public async Task GetUser_ExceptionThrown_ShouldReturnError()
    {
        // Arrange
        var email = "test@example.com";

        _userRepositoryMock.Setup(r => r.GetByEmail(email)).ThrowsAsync(new Exception("Database error"));
        _logHelperMock.Setup(l => l.GetMessage(LoggerUser.NotGetUser)).Returns("Error getting user: ");
        _managerLenguajeMock.Setup(m => m.GetMessage(ErrorUser.UserNotFind)).Returns("User not found");

        // Act
        var result = await _userService.GetUser(email);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.UserDTO);
        Assert.Contains("User not found", result.Message);
    }
}


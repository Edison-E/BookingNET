using AutoMapper;
using BookPro.Application.Features.Authentication.Token;
using BookPro.Application.Features.Token.Interface;
using BookPro.Application.Features.Token.Validations;
using BookPro.Common.Config;
using BookPro.Common.Helper.interfaces;
using BookPro.Common.Localization;
using BookPro.Domain.Entitys.Tokens.Resource;
using BookPro.Domain.interfaces.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookPro.UnitTest.Application;

public class TokenServiceTests
{
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<ITokenValidator> _tokenValidatorMock;
    private readonly Mock<ITokenGenerateFactory> _tokenGenerateFactoryMock;
    private readonly Mock<ILogMessageToken> _logHelperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<TokenService>> _loggerMock;
    private readonly Mock<IManagerResourceLenguaje> _managerLenguajeMock;
    private readonly TokenService _tokenService;
    private readonly SettingsToken _settingsToken;

    public TokenServiceTests()
    {
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _tokenValidatorMock = new Mock<ITokenValidator>();
        _tokenGenerateFactoryMock = new Mock<ITokenGenerateFactory>();
        _logHelperMock = new Mock<ILogMessageToken>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<TokenService>>();
        _managerLenguajeMock = new Mock<IManagerResourceLenguaje>();
        _settingsToken = new SettingsToken { IP = "127.0.0.1" };

        var options = Options.Create(_settingsToken);

        _tokenService = new TokenService(
            options,
            _mapperMock.Object,
            _loggerMock.Object,
            _refreshTokenRepositoryMock.Object,
            _tokenValidatorMock.Object,
            _tokenGenerateFactoryMock.Object,
            _managerLenguajeMock.Object,
            _logHelperMock.Object
        );
    }

    [Fact]
    public async Task GetAccessToken_UserHasNoRefreshToken_ShouldGenerateNewToken()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        _refreshTokenRepositoryMock.Setup(r => r.GetLastRefreshTokenByUser(user.Id)).ReturnsAsync((RefreshToken)null);
        _tokenGenerateFactoryMock.Setup(t => t.GenerateRefreshToken()).Returns("new_refresh_token");
        _tokenGenerateFactoryMock.Setup(t => t.GenerateAccessToken(user.Email)).Returns("new_access_token");
        _refreshTokenRepositoryMock.Setup(r => r.InsertToken(It.IsAny<RefreshToken>())).ReturnsAsync(true);
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessToken.TokenGenerate)).Returns("Token generated successfully");

        // Act
        var result = await _tokenService.GetAccessToken(user);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("new_access_token", result.TokenAccess);
        Assert.Contains("Token generated successfully", result.Message);
    }

    [Fact]
    public async Task GetAccessToken_RefreshTokenIsExpired_ShouldGenerateNewToken()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        var expiredToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Expires = DateTime.UtcNow.AddMinutes(-1) };
        _refreshTokenRepositoryMock.Setup(r => r.GetLastRefreshTokenByUser(user.Id)).ReturnsAsync(expiredToken);
        _tokenValidatorMock.Setup(v => v.ValidationRefreshToken(expiredToken)).Returns(new RefreshTokenValidationResult { IsExpired = true });
        _tokenGenerateFactoryMock.Setup(t => t.GenerateRefreshToken()).Returns("new_refresh_token");
        _tokenGenerateFactoryMock.Setup(t => t.GenerateAccessToken(user.Email)).Returns("new_access_token");
        _refreshTokenRepositoryMock.Setup(r => r.UpdateTokenRevoked(It.IsAny<string>(), expiredToken.Id, _settingsToken.IP)).ReturnsAsync(true);
        _refreshTokenRepositoryMock.Setup(r => r.InsertToken(It.IsAny<RefreshToken>())).ReturnsAsync(true);
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessToken.TokenGenerate)).Returns("Token generated successfully");

        // Act
        var result = await _tokenService.GetAccessToken(user);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("new_access_token", result.TokenAccess);
        Assert.Contains("Token generated successfully", result.Message);
    }

    [Fact]
    public async Task GetAccessToken_RefreshTokenIsValid_ShouldReturnAccessToken()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        var validToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Expires = DateTime.UtcNow.AddMinutes(10) };
        _refreshTokenRepositoryMock.Setup(r => r.GetLastRefreshTokenByUser(user.Id)).ReturnsAsync(validToken);
        _tokenValidatorMock.Setup(v => v.ValidationRefreshToken(validToken)).Returns(new RefreshTokenValidationResult { IsExpired = false });
        _tokenGenerateFactoryMock.Setup(t => t.GenerateAccessToken(user.Email)).Returns("access_token");
        _managerLenguajeMock.Setup(m => m.GetMessage(SuccessToken.TokenGenerate)).Returns("Token generated successfully");

        // Act
        var result = await _tokenService.GetAccessToken(user);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("access_token", result.TokenAccess);
        Assert.Contains("Token generated successfully", result.Message);
    }

    [Fact]
    public async Task GetAccessToken_ExceptionThrown_ShouldReturnError()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        _refreshTokenRepositoryMock.Setup(r => r.GetLastRefreshTokenByUser(user.Id)).ThrowsAsync(new Exception("Database error"));
        _managerLenguajeMock.Setup(m => m.GetMessage(ErrorToken.ErrorCreateAccessToken)).Returns("Error creating access token");

        // Act
        var result = await _tokenService.GetAccessToken(user);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Error creating access token", result.Message);
    }
}

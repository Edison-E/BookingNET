using BookPro.Common.Config;
using BookPro.Domain.Entitys.Tokens.logger;
using System.Text;

namespace BookPro.Application.Features.Authentication.Token;

public class TokenService : ServiceBase, ITokenService
{
    private readonly SettingsToken _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenValidator _tokenValidator;
    private readonly ITokenGenerateFactory _tokenGenerateFactory;
    private readonly ILogMessageToken _logHelper;

    public TokenService(
        IOptions<SettingsToken> configuration,
        IMapper mapper,
        ILogger<TokenService> logger,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenValidator tokenValidator,
        ITokenGenerateFactory tokenGenerateFactory,
        IManagerResourceLenguaje managerLenguaje,
        ILogMessageToken logHelper)
        : base(mapper, logger, managerLenguaje)
    {
        _configuration = configuration.Value;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenValidator = tokenValidator;
        _tokenGenerateFactory = tokenGenerateFactory;
        _logHelper = logHelper;
    }

    public async Task<TokenResponseDTO> GetAccessToken(User user)
    {
        try
        {
            RefreshToken tokenRefresh = await _refreshTokenRepository.GetLastRefreshTokenByUser(user.Id);

            if (tokenRefresh == null)
            {
                return await HandleRefreshTokenNull(user);
            }


            var refreshTokenValidation = _tokenValidator.ValidationRefreshToken(tokenRefresh);

            if (refreshTokenValidation.IsExpired)
            {
                return await HandleRefreshTokenExpire(tokenRefresh, user);
            }


            return await HandlerRefreshTokenNotNull(user, refreshTokenValidation);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerToken.ErrorCreateAccessToken), ex);
            return TokenResponseFactory.CreateTokenResponse(false, new List<string> { _managerLenguaje.GetMessage(ErrorToken.ErrorCreateAccessToken) }, "");
        }
    }
    private async Task<bool> RegisterToken(RefreshToken token) => await _refreshTokenRepository.InsertToken(token);
    public ClaimsPrincipal ValidateResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.Key);

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }
    private RefreshToken CreateRefreshToken(int userId)
    {
        RefreshToken refreshToken = new RefreshToken();
        try
        {
            string tokenGenerate = _tokenGenerateFactory.GenerateRefreshToken();

            var resultTokenGenerate = _tokenValidator.ValidationStringToken(tokenGenerate);

            if (!resultTokenGenerate.Valid)
            {
                LogWarning(_logHelper.GetMessage(LoggerToken.ErrorGenerateStringRefreshToken));
                return refreshToken;
            }

            refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = tokenGenerate,
                Expires = DateTime.UtcNow.AddMinutes(1),
                Created = DateTime.UtcNow,
                Revoked = null,
                CreatedByIp = _configuration.IP,
                RevokedByIp = "",
                ReplacedByToken = "",
                UserId = userId
            };

            return refreshToken;
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerToken.ErrorGenerateModelRefreshToken), ex);
            return refreshToken;
        }
    }
    private async Task<TokenResponseDTO> HandleRefreshTokenNull(User user)
    {
        var tokenRefreshGenerate = CreateRefreshToken(user.Id);
        tokenRefreshGenerate.Token = CryptHelper.EncryptString(tokenRefreshGenerate.Token, "sZzgXzVy7TsujN65EFLkKH8MfjXDJQnvTKq/koW0rkM=", "ESsfPTP3LjTtI9IZEqrdSw==");

        bool isRegisterToken = await RegisterToken(tokenRefreshGenerate);

        if (!isRegisterToken)
        {
            return TokenResponseFactory.CreateTokenResponse(isRegisterToken, new List<string> { _managerLenguaje.GetMessage(ErrorToken.TokenNotRegister) }, "");
        }

        string tokenAccess = _tokenGenerateFactory.GenerateAccessToken(user.Email);

        if (tokenAccess == null)
        {
            return TokenResponseFactory.CreateTokenResponse(true, new List<string> { _managerLenguaje.GetMessage(ErrorToken.TokenNotGenerate) }, "");
        }

        return TokenResponseFactory.CreateTokenResponse(true, new List<string> { _managerLenguaje.GetMessage(SuccessToken.TokenGenerate) }, tokenAccess);
    }
    private async Task<TokenResponseDTO> HandleRefreshTokenExpire(RefreshToken tokenRefresh, User user)
    {
        string newRefreshToken = _tokenGenerateFactory.GenerateRefreshToken();
        bool updateOldToken = await _refreshTokenRepository.UpdateTokenRevoked(newRefreshToken, tokenRefresh.Id, _configuration.IP);

        RefreshToken newTokenRefresh = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = CryptHelper.EncryptString(newRefreshToken, "sZzgXzVy7TsujN65EFLkKH8MfjXDJQnvTKq/koW0rkM=", "ESsfPTP3LjTtI9IZEqrdSw=="),
            Expires = DateTime.UtcNow.AddMinutes(1),
            Created = DateTime.UtcNow,
            Revoked = null,
            CreatedByIp = _configuration.IP,
            RevokedByIp = "",
            ReplacedByToken = "",
            UserId = tokenRefresh.UserId,
        };

        bool isRegisterToken = await _refreshTokenRepository.InsertToken(newTokenRefresh);

        if (!isRegisterToken)
        {
            return TokenResponseFactory.CreateTokenResponse(false, new List<string> { _managerLenguaje.GetMessage(ErrorToken.TokenNotUpdate) }, "");
        }

        string tokenAccess = _tokenGenerateFactory.GenerateAccessToken(user.Email);

        return TokenResponseFactory.CreateTokenResponse(true, new List<string> { _managerLenguaje.GetMessage(SuccessToken.TokenGenerate) }, tokenAccess);
    }
    private async Task<TokenResponseDTO> HandlerRefreshTokenNotNull(User user, ValidationResult refreshTokenValidation)
    {
        if (!refreshTokenValidation.Valid)
        {
            return TokenResponseFactory.CreateTokenResponse(refreshTokenValidation.Valid, new List<string> { _managerLenguaje.GetMessage(ErrorToken.TokenInvalid) }, "");
        }

        string tokenAccess = _tokenGenerateFactory.GenerateAccessToken(user.Email);

        if (tokenAccess == null)
        {
            return TokenResponseFactory.CreateTokenResponse(true, new List<string> { _managerLenguaje.GetMessage(ErrorToken.TokenNotGenerate) }, "");
        }


        return TokenResponseFactory.CreateTokenResponse(true, new List<string> { _managerLenguaje.GetMessage(SuccessToken.TokenGenerate) }, tokenAccess);
    }
}

using BookPro.Common.Config;

namespace BookPro.Application.Features.Token.Factory;

public class TokenGenerateFactory : ITokenGenerateFactory
{
    private readonly SettingsToken _configuration;

    public TokenGenerateFactory(IOptions<SettingsToken> configuration)
    {
        _configuration = configuration.Value;
    }

    public string GenerateAccessToken(string email)
    {
        string accessToken = string.Empty;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", email) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration.Issuer,
                Audience = _configuration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }
        catch (Exception ex)
        {
            return accessToken;
        }
    }

    public string GenerateRefreshToken()
    {
        string tokenGenerate = string.Empty;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(15),
                Issuer = _configuration.Issuer,
                Audience = _configuration.Audience,
                Claims = new Dictionary<string, object>
                {
                    {"Token", Guid.NewGuid().ToString()},
                },
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenGenerate = tokenHandler.WriteToken(token);

            return tokenGenerate;
        }
        catch (Exception ex)
        {
            return tokenGenerate;
        }

    }

    public string GenerateResetToken(string email)
    {
        string resetToken = string.Empty;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", email) }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = _configuration.Issuer,
                Audience = _configuration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            resetToken = tokenHandler.WriteToken(token);

            return resetToken;
        }
        catch (Exception ex)
        {
            return resetToken;
        }
    }
}

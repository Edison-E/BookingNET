using BookPro.Common.Helper.interfaces;
using BookPro.Domain.Entitys.Tokens.logger;

namespace BookPro.Common.Helper;

public class LogMessageToken : ILogMessageToken
{
    private readonly Dictionary<LoggerToken, string> Messages = new()
    {
        { LoggerToken.ErrorGenerateStringRefreshToken, "The refresh token has not been generated correctly." },
        { LoggerToken.ErrorGenerateModelRefreshToken, "A ocurred while generate the refresh token."},
        { LoggerToken.ErrorCreateAccessToken, "A ocurred while create access token."}
    };

    public string GetMessage(LoggerToken token)
    {
        return Messages[token].ToString();
    }
}

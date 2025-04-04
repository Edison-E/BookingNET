using BookPro.Common.Logger.interfaces;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Common.Logger;

public class LogMessageUser : ILogMessageUser
{
    private readonly Dictionary<LoggerUser, string> Message = new()
    {
        { LoggerUser.NotFindUser, "No user find with this email " },
        { LoggerUser.NotGetUser, "A ocurred while get user with this email " },
        { LoggerUser.NotUpdateUser, "The password could not be updated correctly." },
        { LoggerUser.TokenInvalid, "The token is invalid." }
    };

    public string GetMessage(LoggerUser token)
    {
        return Message[token];
    }
}

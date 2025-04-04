using BookPro.Domain.Entitys.Tokens;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Common.Logger.interfaces;

public interface ILogMessageUser
{
    string GetMessage(LoggerUser token);
}

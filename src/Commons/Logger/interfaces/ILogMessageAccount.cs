using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Common.Helper.interfaces;

public interface ILogMessageAccount
{
    string GetMessage(LoggerAccount user);
}

using BookPro.Domain.Entitys.Tokens.logger;

namespace BookPro.Common.Helper.interfaces;

public interface ILogMessageToken
{
    string GetMessage(LoggerToken token);
}

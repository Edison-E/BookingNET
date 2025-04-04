using BookPro.Common.Helper.interfaces;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Common.Helper;

public class LogMessageAccount : ILogMessageAccount
{
    private readonly Dictionary<LoggerAccount, string> Messages = new()
    {
        { LoggerAccount.GenerateTokenUser, "Something happened while we were generating the user´s token" },
        { LoggerAccount.AccountExistUser, "An account with provided email already exists" },
        { LoggerAccount.ErrorRegisterUser, "An ocurred while register the user." },
        { LoggerAccount.ErrorVerifingCredentials, "Error: An ocurred while verifing credentials for email " },
        { LoggerAccount.NotUpdatePassword, "The password could not be updated correctly."},
        { LoggerAccount.ErrorProccessResetPassword, "Something happened while resetting the password."}
    };

    public string GetMessage(LoggerAccount user)
    {
        return Messages[user].ToString();
    }
}

namespace BookPro.Domain.Entitys.Users.Resource;

public enum ErrorUser
{
    UserNotRegister,
    UserVerifingCredentials,
    UserInvalid,
    UserNotFind,
    AccountExist,
    UserErrorUpdatePassword,
    ErrorProccessResetPassword,
    TokenResetInvalid
}

using BookPro.Application.Features.Authentication.Account.DTOs;
using BookPro.Application.Features.Authentication.Account.Interface;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Application.Features.Authentication.Account;

public class AccountService : ServiceBase, IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountValidator _accountValidator;
    private readonly IUserValidator _userValidation;
    private readonly ITokenService _tokenService;
    private readonly ILogMessageAccount _logHelper;
    private readonly ILogMessageUser _logHelperUser;
    private readonly IEmailServices _emailService;
    private readonly ITokenGenerateFactory _tokenGenerateFactory;
    private readonly int MAX_ATTEMPTS = 3;
    private readonly int RESET_ATTEMPTS = 0;

    public AccountService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<AccountService> logger,
        IAccountValidator accountValidation,
        IUserValidator userValidation,
        ITokenService tokenService,
        IManagerResourceLenguaje managerLenguaje,
        ILogMessageAccount logHelper,
        ILogMessageUser logHelperUser,
        IEmailServices emailServices,
        ITokenGenerateFactory tokenGenerateFactory) : base(mapper, logger, managerLenguaje)
    {
        _userRepository = userRepository;
        _accountValidator = accountValidation;
        _userValidation = userValidation;
        _tokenService = tokenService;
        _logHelper = logHelper;
        _logHelperUser = logHelperUser;
        _emailService = emailServices;
        _tokenGenerateFactory = tokenGenerateFactory;
    }

    public async Task<LoginResponseDTO> Login(LoginRequest loginRequest)
    {
        ResponseDTO loginResponse;
        try
        {
            var isValidRequest = _accountValidator.ValidationLogin(loginRequest);
            if (!isValidRequest.Valid)
            {
                LogWarning(isValidRequest.GetMessageAsString());
                return AccountResponseFactory.CreateLoginResponse(isValidRequest.Message, "", isValidRequest.Valid);
            }

            User user = await _userRepository.GetByEmail(loginRequest.Email);
            var userIsValid = _userValidation.ValidateUser(user);
            if (!userIsValid.Valid)
            {
                LogWarning("The account not exist, this email.");
                return AccountResponseFactory.CreateLoginResponse(new List<string> { "The account not exist, this email." }, "", false);
            }

            if (user.FailedLoginAttempts == MAX_ATTEMPTS)
            {
                LogWarning("The account is blocked");
                return AccountResponseFactory.CreateLoginResponse(new List<string> { "The account is blocked" }, "", false);
            }

            loginResponse = await VerifyCredentials(user, loginRequest);
            if (!loginResponse.Success)
            {
                LogWarning(loginResponse.GetMessage());
                return AccountResponseFactory.CreateLoginResponse(loginResponse.Message, "", loginResponse.Success);
            }

            TokenResponseDTO tokenResponse = await _tokenService.GetAccessToken(user);
            if (!tokenResponse.Success)
            {
                return AccountResponseFactory.CreateLoginResponse(tokenResponse.Message, "", tokenResponse.Success);
            }

            loginResponse.Message.AddRange(tokenResponse.Message);

            return AccountResponseFactory.CreateLoginResponse(loginResponse.Message, tokenResponse.TokenAccess, loginResponse.Success);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerAccount.GenerateTokenUser), ex);
            return AccountResponseFactory.CreateLoginResponse(new List<string> { _managerLenguaje.GetMessage(ErrorToken.HappenedTokenError) }, "", false);
        }
    }
    
    public async Task<RegisterResponseDTO> Register(RegisterRequest register)
    {
        try
        {
            ValidationResult validationParameters = _accountValidator.ValidationRegister(register);
            if (!validationParameters.Valid)
            {
                LogWarning(validationParameters.GetMessageAsString());
                return AccountResponseFactory.CreateRegisterResponse(validationParameters.Message, validationParameters.Valid);
            }

            if (await AccountExist(register.Email))
            {
                LogWarning(_logHelper.GetMessage(LoggerAccount.AccountExistUser));
                return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(ErrorUser.AccountExist) }, false);
            }

            register.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);
            User user = _mapper.Map<User>(register);
            bool isInsertUser = await _userRepository.Insert(user);

            if (!isInsertUser)
            {
                return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(ErrorUser.UserNotRegister) }, isInsertUser);
            }

            //await _emailService.SendEmailAsync(register.Email,"Cuenta creada","<h2>Bienvenido a BookPro</h2>");
            return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(SuccessUser.UserRegister) }, validationParameters.Valid);

        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerAccount.ErrorRegisterUser), ex);
            return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(ErrorUser.UserNotRegister) }, false);
        }
    }
   
    public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        try
        {
            string token = _tokenGenerateFactory.GenerateResetToken(request.Email);
            string link = $"http://localhost:8080/Reset?token={token}";

            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Features", "Authentication", "Account", "TemplateEmail", "ResetPassword.html");
            string body = await File.ReadAllTextAsync(templatePath);

            body = body.Replace("{{Email}}", request.Email).Replace("{{ResetLink}}", link);

            await _emailService.SendEmailAsync(request.Email, "Restablecer contraseña", body);

            return AccountResponseFactory.CreateResetForgotPassword(token, true);  
        }
        catch (Exception ex)
        {
            LogError(_logHelperUser.GetMessage(LoggerUser.TokenInvalid), ex);
            return AccountResponseFactory.CreateResetForgotPassword("", false); 
        }
    }
    
    public async Task<ResetPasswordResponseDTO> ResetPassword(ResetPasswordRequest resetPassword)
    {
        try
        {
            var claims = _tokenService.ValidateResetToken(resetPassword.Token);
            if (claims is null)
            {
                LogWarning(_logHelperUser.GetMessage(LoggerUser.TokenInvalid));
                return AccountResponseFactory.CreateResetPasswordResponseDTO(new List<string> { _managerLenguaje.GetMessage(ErrorUser.TokenResetInvalid) }, false);
            }

            string email = claims.FindFirst(ClaimTypes.Email).Value;
            var user = await _userRepository.GetByEmail(email);

            var userIsValid = _userValidation.ValidateUser(user);
            if (!userIsValid.Valid)
            {
                LogWarning(_logHelperUser.GetMessage(LoggerUser.NotFindUser));
                return AccountResponseFactory.CreateResetPasswordResponseDTO(new List<string> { _managerLenguaje.GetMessage(ErrorUser.UserNotFind) }, false);
            }

            var responseUpdatePassword = await UpdatePasswordForUser(user, resetPassword.NewPassword);

            return AccountResponseFactory.CreateResetPasswordResponseDTO(new List<string> { responseUpdatePassword }, true);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerAccount.ErrorProccessResetPassword), ex);
            return AccountResponseFactory.CreateResetPasswordResponseDTO(new List<string> { _managerLenguaje.GetMessage(ErrorUser.ErrorProccessResetPassword) }, false);
        }
    }
    


    private async Task<ResponseDTO> VerifyCredentials(User user, LoginRequest login)
    {
        ValidationResult response = new ValidationResult();
        try
        {
            response = _accountValidator.ValidationCredentials(login, user);
            if (!response.Valid)
            {
                return await UpdateAttemptsForUser(user, response);
            }

            return ResponseDTOFactory.CreateSuccess(new List<string> { _managerLenguaje.GetMessage(SuccessUser.UserCredentials) }, "");
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerAccount.ErrorVerifingCredentials) + login.Email + ".", ex);
            return ResponseDTOFactory.CreateError(
                new List<string>() { _managerLenguaje.GetMessage(ErrorUser.UserVerifingCredentials) + login.Email + ". " }, "Error");
        }
    }
   
    private async Task<bool> AccountExist(string email) => await _userRepository.GetByEmail(email) != null;

    private async Task<ResponseDTO> UpdateAttemptsForUser(User user, ValidationResult response)
    {
        user.FailedLoginAttempts += 1;
        user.LastFailedLoginAttempt = DateTime.Now;

        var isUpdateAttempts = await _userRepository.Update(user);
        if (!isUpdateAttempts)
        {
            response.Message.AddRange(new List<string> { "Not update attempts the user" });
            LogWarning(response.GetMessageAsString());
            return ResponseDTOFactory.CreateError(response.Message, "Critico");
        }

        LogWarning(response.GetMessageAsString());
        return ResponseDTOFactory.CreateError(response.Message, "Critico");
    }
   
    private async Task<string> UpdatePasswordForUser(User user, string newPassword)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.FailedLoginAttempts = RESET_ATTEMPTS;

        var isUpdateUser = await _userRepository.Update(user);
        if (!isUpdateUser)
        {
            LogWarning(_logHelper.GetMessage(LoggerAccount.NotUpdatePassword));
            return _managerLenguaje.GetMessage(ErrorUser.UserErrorUpdatePassword);
        }

        return _managerLenguaje.GetMessage(SuccessUser.UserUpdatePassword);
    }
}

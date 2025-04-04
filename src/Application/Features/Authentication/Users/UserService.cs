using BookPro.Application.Features.Authentication.Users.DTO;
using BookPro.Application.Features.Authentication.Users.Factory;
using BookPro.Domain.Entitys.Users.logger;

namespace BookPro.Application.Features.Authentication.Users;

public class UserService : ServiceBase, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidation;
    private readonly ILogMessageUser _logHelper;
    public UserService(
        IMapper mapper,
        ILogger<UserService> logger,
        IUserRepository userRepository,
        IUserValidator userValidation,
        IManagerResourceLenguaje managerLenguaje,
        ILogMessageUser logHelper)
        : base(mapper, logger, managerLenguaje)
    {
        _userRepository = userRepository;
        _userValidation = userValidation;
        _logHelper = logHelper;
    }

    public async Task<UserResponseDTO> GetUser(string email)
    {
        UserDTO userDTO = null;
        try
        {
            User user = await _userRepository.GetByEmail(email);

            ValidationResult result = _userValidation.ValidateUser(user);

            if (!result.Valid)
            {
                LogWarning(_logHelper.GetMessage(LoggerUser.NotFindUser) + email + ".");
                return UserResponseFactory.CreateUserResponse(new List<string> { _managerLenguaje.GetMessage(ErrorUser.UserInvalid) }, userDTO, result.Valid);
            }

            userDTO = _mapper.Map<UserDTO>(user);
            return UserResponseFactory.CreateUserResponse(new List<string> { _managerLenguaje.GetMessage(SuccessUser.UserFind) }, userDTO, result.Valid);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerUser.NotGetUser) + email + ".", ex);
            return UserResponseFactory.CreateUserResponse(new List<string> { _managerLenguaje.GetMessage(ErrorUser.UserNotFind) }, userDTO, false);
        }
    }
}

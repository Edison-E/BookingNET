namespace BookPro.Application.Features.Authentication.Users.Interface;
public interface IUserValidator : IValidator<User>
{
    ValidationResult ValidateUser(User user);
}

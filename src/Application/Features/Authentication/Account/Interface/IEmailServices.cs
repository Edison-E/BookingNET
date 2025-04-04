namespace BookPro.Application.Features.Authentication.Account.Interface;

public interface IEmailServices
{
    Task SendEmailAsync(string to, string subject, string body);
}

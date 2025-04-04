namespace BookPro.Domain.interfaces.Authentication;

public interface IUserRepository
{
    Task<User> GetByEmail(string email);
    Task<User> GetById(int id);
    Task<bool> Insert(User register);
    Task<bool> Update(User user);
}

namespace BookPro.Infrastructure.Repositories.Authentication;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<User> GetByEmail(string email) => _context.Users.FirstOrDefault(x => x.Email == email);
    
    public async Task<User> GetById(int id) => await base.GetById(id);
    
    public async Task<bool> Insert(User register)
    {
        try
        {
            return await base.Insert(register);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> Update(User user) => await base.Update(user, user.Id);
}

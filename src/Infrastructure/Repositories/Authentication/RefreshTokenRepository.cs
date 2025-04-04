namespace BookPro.Infrastructure.Repositories.Authentication;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken> GetTokenByUser(int id) =>
        await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);

    public async Task<RefreshToken> GetLastRefreshTokenByUser(int id)
    {
        return await _context.RefreshTokens.Where(token => token.UserId == id)
             .OrderByDescending(token => token.Created)
             .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<RefreshToken>> GetAllTokenByUser(int id) =>
        await _context.RefreshTokens.Where(x => x.UserId == id).ToListAsync();

    public async Task<bool> InsertToken(RefreshToken token) => await Insert(token);

    public async Task<bool> DeleteToken(int id) => await Delete(id);

    public async Task<bool> UpdateTokenRevoked(string newRefreshToken, Guid id, string IP)
    {
        var oldRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Id == id);

        if (oldRefreshToken == null)
        {
            return false;
        }

        oldRefreshToken.Revoked = DateTime.UtcNow;
        oldRefreshToken.RevokedByIp = IP;
        oldRefreshToken.ReplacedByToken = newRefreshToken;

        return await _context.SaveChangesAsync() > 0;
    }

}

namespace BookPro.Domain.interfaces.Authentication;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetTokenByUser(int id);
    Task<RefreshToken> GetLastRefreshTokenByUser(int id);
    Task<IEnumerable<RefreshToken>> GetAllTokenByUser(int id);
    Task<bool> DeleteToken(int id);
    Task<bool> InsertToken(RefreshToken token);
    Task<bool> UpdateTokenRevoked(string newRefreshToken, Guid id, string IP);
}
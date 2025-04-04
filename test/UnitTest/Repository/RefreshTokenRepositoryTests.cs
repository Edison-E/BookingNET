using BookPro.Infrastructure.Repositories.Authentication;

namespace BookPro.UnitTest.Repository;

public class RefreshTokenRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly RefreshTokenRepository _repository;

    public RefreshTokenRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new RefreshTokenRepository(_context);
    }

    [Fact]
    public async Task GetTokenByUser_TokenExists_ShouldReturnToken()
    {
        // Arrange
        var userId = 1;
        var refreshToken = new RefreshToken { 
            UserId = userId, 
            Token = Guid.NewGuid().ToString(), 
            CreatedByIp = "127.0.0.1",
            ReplacedByToken = "", 
            RevokedByIp = ""
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTokenByUser(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public async Task GetLastRefreshTokenByUser_TokenExists_ShouldReturnLastToken()
    {
        // Arrange
        var userId = 1;
        var refreshTokens = new List<RefreshToken> {
            new RefreshToken { 
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow.AddDays(-1),
                CreatedByIp = "127.0.0.1",
                ReplacedByToken = "",
                RevokedByIp = "" 
            },
            new RefreshToken
            {
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow.AddDays(-2),
                CreatedByIp = "127.0.0.1",
                ReplacedByToken = "",
                RevokedByIp = ""
            }
        };

        _context.RefreshTokens.AddRange(refreshTokens);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetLastRefreshTokenByUser(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
      
    }

    [Fact]
    public async Task InsertToken_ShouldInsertToken()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            Token = "new-token-unit-test",
            Created = DateTime.UtcNow,
            CreatedByIp = "127.0.0.1",
            ReplacedByToken = "",
            RevokedByIp = ""
        };

        // Act
        var result = await _repository.InsertToken(refreshToken);
        var tokenSaved = _context.RefreshTokens.FindAsync(refreshToken.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(tokenSaved);
    }

    [Fact]
    public async Task DeleteToken_TokenExists_ShouldDeleteToken()
    {
        // Arrange
        
        // Act

        // Assert
    }

    [Fact]
    public async Task UpdateTokenRevoked_TokenExists_ShouldUpdateToken()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken> {
            new RefreshToken {
                Id =  Guid.NewGuid(),
                UserId = 1,
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow.AddDays(-1),
                CreatedByIp = "127.0.0.1",
                ReplacedByToken = "",
                RevokedByIp = ""
            },
            new RefreshToken
            {
                Id =  Guid.NewGuid(),
                UserId = 1,
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow.AddDays(-2),
                CreatedByIp = "127.0.0.1",
                ReplacedByToken = "",
                RevokedByIp = ""
            }
        };

        _context.RefreshTokens.AddRange(refreshTokens);
        await _context.SaveChangesAsync();

        var refreshToken = await _repository.GetLastRefreshTokenByUser(1);
        string newRefreshToken = Guid.NewGuid().ToString();
        string ip = "127.0.0.1";


        // Act
        var result = await _repository.UpdateTokenRevoked(newRefreshToken, refreshToken.Id, ip);
        var updateToken = await _repository.GetLastRefreshTokenByUser(refreshToken.UserId);

        // Assert
        Assert.True(result);
        Assert.Equal(refreshToken.ReplacedByToken, newRefreshToken);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

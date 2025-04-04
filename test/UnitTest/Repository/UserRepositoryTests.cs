using BookPro.Infrastructure.Repositories.Authentication;

namespace BookPro.UnitTest.Repository;

public class UserRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _userRepository;
    public UserRepositoryTests() 
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetUserByEmail_UserExists_ShouldReturnUser()
    {
        // Arrange
        string email = "ecrespo@gmail.com";
        var user = new User
        {
            Id = 1,
            Name = "Test",
            Email = email,
            Password = "testPassword",
            PhoneNumber = 649922682,
            DateBirth = "01/06/2002",
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetByEmail(email);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserByEmail_UserNotExist_ShouldReturnNull()
    {
        // Arrange
        string email = "testEmail@gmail.com";

        // Act
        var result = await _userRepository.GetByEmail(email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserById_UserExist_ShouldReturnUser()
    {
        // Arrange
        int userId = 1;
        var user = new User
        {
            Id = 1,
            Name = "Test",
            Email = "testId@gmail.com",
            Password = "testPassword",
            PhoneNumber = 649922682,
            DateBirth = "01/06/2002",
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetById(userId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserById_UserExist_ShouldReturnNull()
    {
        // Arrange
        int userId = 1;
      
        // Act
        var result = await _userRepository.GetById(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertUser_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "Test",
            Email = "testInsert@gmail.com",
            Password = "testPassword",
            PhoneNumber = 649922682,
            DateBirth = "01/06/2002",
        };

        // Act
        var result = await _userRepository.Insert(user);
        var userSaved = await _userRepository.GetByEmail(user.Email);

        // Assert
        Assert.True(result);
        Assert.NotNull(userSaved);
    }

    [Fact]
    public async Task InsertUser_ShouldReturnFalse()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = null,
            Email = null,
            Password = null,
            PhoneNumber = 0,
            DateBirth = null
        };

        // Act
        var result = await _userRepository.Insert(null);

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

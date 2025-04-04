
using BookPro.Infrastructure.Repositories.Authentication;

namespace BookPro.UnitTest.Repository;

public class RepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Repository<User> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

        _context = new ApplicationDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetbyId_EntityExist_ShouldReturnEntity()
    {
        // Arrange
        var id = 1;
        var entity = new User
        {
            Id = 1,
            Name = "Test",
            Email = "testEntity@gmail.com",
            Password = "testPassword",
            PhoneNumber = 649922682,
            DateBirth = "01/06/2002",
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(id);

        // Assert
        Assert.NotNull(result);

    }

    [Fact]
    public async Task GetbyId_EntityNotExist_ShouldReturnNull()
    {
        // Arrange
        var id = 3;
        var entitysDataBase = new List<User> {
            new User {
                Id = 1,
                Name = "Test",
                Email = "testEntity@gmail.com",
                Password = "testPassword",
                PhoneNumber = 649922682,
                DateBirth = "01/06/2002",
            },
            new User
            {
                Id = 2,
                Name = "Test",
                Email = "testEntity@gmail.com",
                Password = "testPassword",
                PhoneNumber = 649922682,
                DateBirth = "01/06/2002",
            }
        };

        _context.Users.AddRange(entitysDataBase);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertEntity_ShouldReturnTrue()
    {
        // Arrange
        var newEntity = new User
        {
            Id = 1,
            Name = "Test",
            Email = "testEntity@gmail.com",
            Password = "testPassword",
            PhoneNumber = 649922682,
            DateBirth = "01/06/2002",
        };

        // Act
        var result = await _repository.Insert(newEntity);
        var entitySaved = await _repository.GetById(newEntity.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(entitySaved);
    }

    [Fact]
    public async Task InsertEntity_ShouldReturnFalse()
    {
        // Arrange
        var newEntity = new User
        {
            Id = 1,
            Name = null,
            Email = null,
            Password = null,
            PhoneNumber = 0,
            DateBirth = null,
        };

        // Act
        var result = await _repository.Insert(newEntity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateEntity_ShouldReturnTrue()
    {
        // Arrange
        var id = 1;
        var updateEntity = new User
        {
            Id = 1,
            Name = "TestUpdate",
            Email = "testUpdate@gmail.com",
            Password = "testUpdate",
            PhoneNumber = 670934932,
            DateBirth = "02/06/2002"
        };

        var entitysDataBase = new List<User> {
            new User {
                Id = 1,
                Name = "Test",
                Email = "testEntity@gmail.com",
                Password = "testPassword",
                PhoneNumber = 649922682,
                DateBirth = "01/06/2002",
            },
            new User
            {
                Id = 2,
                Name = "Test",
                Email = "testEntity@gmail.com",
                Password = "testPassword",
                PhoneNumber = 649922682,
                DateBirth = "01/06/2002",
            }
        };

        _context.Users.AddRange(entitysDataBase);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.Update(updateEntity, id);

        // Assert
        Assert.True(result);
        Assert.Equal(updateEntity.Name, entitysDataBase[0].Name);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}


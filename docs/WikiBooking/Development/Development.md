# Development
## Development Environment Configuration
### Prerequisite
In order to develop a new functionality, you should consider the following necessary tools:
- Lenguaje: C#.
- Frameworks: .NET 6.0.
- Tools: 
  - Have Visual Studio 2022 installed.
  - To query the data, you can use SQL Server Management Studio or Visual Studio directly.
  - Postman to test requests or Swagger.
  - Git for version control, or you can use the graphical environment integrated in Visual Studio.

&nbsp;
## Folder organization
To start a new development, you must take into account how the folders are organized.

It is organized as follows:
`API` 
- 📂 BookPro.API
  - 📂 Properties
  - 📂 Features
     - 📂 Account
     - 📂 Users
  - 📄 Program.cs

`Application` 
- 📂 BookPro.Application
   -  📂 Features
          - 📂 Account
       - 📂 DTOs
       - 📂 Factory
       - 📂 Interface
       - 📂 MappProfiles
       - 📂 Validations
        - 📄 AccountService.cs
    - 📄 ServiceBase.cs
    - 📂 Validation
        - 📂 Interface
        - 📄 BaseValidation.cs
        - 📄 ValidationResult.cs

`Common` 
- 📂 BookPro.Common
  - 📂 Config
  - 📂 Localization
  - 📂 Logger
  - 📂 Interfaces
  - 📂 Resource
  - 📂 Response
  - 📄 ConfigurationService.cs

`Infrastructure`
- 📂 BookPro.Infrastructure
  - 📂 Migrations
  - 📂 Persistence
  - 📂 Repositories
  - 📄 CryHelper.cs
  - 📄 DependencyInjection.cs

`Domain` 
- 📂 BookPro.Domain
  - 📂 Interfaces
  - 📂 Models
     - 📂 Tokens
     - 📂 Users


&nbsp;
## Development Flow
The development flow for adding a new functionality to the system is explained below.

``1. Database management `` 
If the new functionality requires a model, we must first create it in the following path: 
``BookPro.API``/Models/{NameModel}/{NameModel}.cs. 

Then, to reflect the model in the database, it is necessary to register it in the data context.

**1.1 Agregar el modelo a la base de datos** 
Go to ``BookPro.Infrastructure``/Persistence/ApplicationDbContext.cs and add the new model inside the ApplicationDbContext class, as shown below:
````
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<NewModel> NewModel {get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);
    }
}
````
**1.2 Create and apply Migration** 
After updating the database context, we generate and apply the migration using the following commands:
````
Terminal
> dotnet ef migrations add "NameMigration"
> dotnet ef migrations add AddNewModel
> dotnet ef database update

Package Manager Console
> Add-Migration "NameMigration"
> Add-Migration AddNewModel
> Update-Database
````

``2. Database Access `` 
After performing the migration and verifying that the model is reflected in the database, we must create the corresponding repository.

**2.1 Repository Creation** 
1- Create the {IModelRepository.cs} interface, where the model-specific methods will be defined.
2- Create the class {ModelRepository.cs}, which will implement the interface and inherit Repository, which contains the main database access methods.

With these steps, we will have the data access layer ready for the new model.

``3. Service Creation``
Once data access has been implemented, you can proceed to the service layer.

**3.1 Service Structure** 
Creation of main folder and subfolders ``BookPro.Application``/Features.

- 📂 {NameModel}
   -  📂 DTOs
   - 📂 Factory
   - 📂 Interface
   - 📂 Validations
   - 📄 {NameModel}Service.cs

Explanation of each folder:
 - **DTOs** Contains the data transfer objects and response models.
 - **Factory** Defines the static methods for the creation of customized responses.
 - **Interface** Contains the contracts for the classes of the service.
 - **Validations** Implements the model-specific validations.

The {NameModel}Service.cs service must inherit from ServiceBase, which will allow the use of tools such as Mapper, Serilog, ManagerResourceLanguage.
````
public class {NameModel}Service : ServiceBase, I{NameModel}Service
{
    private readonly I{NameModel}Repository _{nameModel}Repository;
    private readonly I{NameModel}Validator _{nameModel}Validation;
    private readonly ILogMessageUser _logHelper;

    public {NameModel}Service(
        IMapper mapper,
        ILogger<{NameModel}Service> logger,
        I{NameModel}Repository {nameModel}Repository,
        I{NameModel}Validator {nameModel}Validation,
        IManagerResourceLenguaje managerLenguaje,
        ILogMessageUser logHelper)
        : base (mapper, logger, managerLenguaje)
    {
        _{nameModel}Repository = {nameModel}Repository;
        _{nameModel}Validation = {nameModel}Validation;
        _logHelper = logHelper;
    }
}
````

``4. Creation of Internationalized Messages``
To manage messages in different languages, follow these steps:

**4.1 Add messages to the resource** 
Go to ``BookPro.Common``/Resource/Response.resx and add a new key with the corresponding message.

**4.2 Create resource class** 
In ``BookPro.Domain``/models/{NameModel}/Resource, here you must create an enum class with the keys defined in the .resx file.

**4.3 Use of the resource in the service**
Within the service, the message is obtained using ManagerResourceLanguage and passing the key defined in the enum:
````
public class {NameModel}Service : ServiceBase, I{NameModel}Service
{
    public async Task<UserResponseDTO> GetUser(string email)
    {
    LogWarning(_logHelper.GetMessage(Logger{nameModel}.{key}) + email + ".");
    return {nameModel}ResponseFactory
    .Create{nameModel}Response(new List<string> {
       _managerLenguaje.GetMessage(Error{nameModel}.{key})}, <--- implementacion de la clase ManagerResourceLenguaje
    }
}
````

```5. Creation of the Controller ```
To expose functionality through an API, you must create a driver in ``BookPro.API``/Features and create the folder and driver:
 - 📂 {NameModel}
   - 📄 {NameModel}Controller.cs 

&nbsp;
## NuGet Usage
In this project we use NuGet packages to facilitate development and implement various functionalities. Some essential packages are:

**Serilog**
With this tool, we will be able to register events, store and visualize logs in a structured way.

```
public abstract class ServiceBase
{
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;
    protected readonly IManagerResourceLenguaje _managerLenguaje;

    public ServiceBase(IMapper mapper, ILogger logger, IManagerResourceLenguaje managerLenguaje)
    {...}

    protected void LogWarning(string message)
    {
        _logger.LogWarning("Warning: " +message);
    }

    protected void LogError(string message, Exception exception)
    {
        _logger.LogError(exception, "Error: " + message);
    }
}
````

**AutoMapper**
This tool facilitates the conversion of objects between different layers of the system.

````
public async Task<RegisterResponseDTO> Register(RegisterDTO register)
{
    try
    {
        ValidationResult validationParameters = _accountValidator.ValidationRegister(register);
        if (!validationParameters.Valid)
        {...}

        if (await AccountExist(register.Email))
        {...}

        register.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);
        User user = _mapper.Map<User>(register); <-- use AutoMapper
        bool isInsertUser = await _userRepository.Insert(user);

        if (!isInsertUser)
        {...}

        return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(SuccessUser.UserRegister) }, validationParameters.Valid);

    }
    catch (Exception ex)
    {...}
}
````

**BCrypt.Net-Next**
With this tool we will be able to perform password encryption or password verification in a secure way.
````
public async Task<RegisterResponseDTO> Register(RegisterDTO register)
{
    try
    {
        ValidationResult validationParameters = _accountValidator.ValidationRegister(register);
        if (!validationParameters.Valid)
        {...}

        if (await AccountExist(register.Email))
        {...}

        register.Password = BCrypt.Net.BCrypt.HashPassword(register.Password); <-- use BCrypt
        User user = _mapper.Map<User>(register); 
        bool isInsertUser = await _userRepository.Insert(user);

        if (!isInsertUser)
        {...}

        return AccountResponseFactory.CreateRegisterResponse(new List<string> { _managerLenguaje.GetMessage(SuccessUser.UserRegister) }, validationParameters.Valid);

    }
    catch (Exception ex)
    {...}
}
````

**Microsoft.AspNetCore.Localization**
With this tool we can manage the localization of the system and define different languages for the application.
Program configuration:
````
//Configure Lenguaje
builder.Services.Configure<SettingsResource>(configuration.GetSection("ResourceLenguaje"));
var supportLenguaje = new[] { "en" , "es" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var lenguajes = supportLenguaje.Select(c => new CultureInfo(c)).ToList();
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
    options.SupportedCultures = lenguajes;
    options.SupportedUICultures = lenguajes;
});

var culture = Thread.CurrentThread.CurrentCulture.Name;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");
````
**Microsoft.EntityFrameworkCore**
This tool provides us with an abstraction to work with databases.
Configuration to access the database:
````
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationService.GetConfiguration(AppContext.BaseDirectory);

        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new ApplicationDbContext(optionBuilder.Options);
    }
}
````

&nbsp;
## Error and Log Handling
Next, we will explain how to handle errors and logs in the application, starting with error handling.

**Error Handling**
Error handling is implemented in both services and repositories. This allows logging important events and having more control over where and why an error occurred.

This would be the implementation of events occurring with the Try Catch:
````
public class UserService : ServiceBase,IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidation;
    private readonly ILogMessageUser _logHelper;
    public UserService(...)
    {...}

    public async Task<UserResponseDTO> GetUser(string email)
    {
        UserDTO userDTO = null;
        try 
        {
            User user = await _userRepository.GetByEmail(email);
            ValidationResult result = _userValidation.ValidateUser(user);

            if (!result.Valid)
            {...}

            userDTO = _mapper.Map<UserDTO>(user);
            return UserResponseFactory.CreateUserResponse(new List<string>{_managerLenguaje.GetMessage(SuccessUser.UserFind) }, userDTO, result.Valid);
        }
        catch (Exception ex)
        {
            LogError(_logHelper.GetMessage(LoggerUser.NotGetUser) + email + ".", ex);
            return UserResponseFactory.CreateUserResponse(new List<string> {_managerLenguaje.GetMessage(ErrorUser.UserNotFind)}, userDTO, false);
        }
    }
}
````

And in the Repository, we use it to check if the write operations are being executed correctly, and in case it is not being done correctly we will return a value.

implementation example:
````
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    public Repository(ApplicationDbContext context)
    {...}

    public async Task<IEnumerable<T>> GetAll()
    {...}

    public async Task<T> GetById(int id) => await _context.Set<T>().FindAsync(id);

    public async Task<bool> Insert(T entity) 
    {
        try
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    } 

    public async Task<bool> Update(T entity, int id)
    {...}

    public async Task<bool> Delete(int id)
    {...}
}
````

**Logs**
To log events and facilitate debugging, we use logs managed in the ServiceBase class, which provides two main methods:
- LogWarning where we will pass the message.
- LogError where we pass the exception and the message.

````
public abstract class ServiceBase
{
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;
    protected readonly IManagerResourceLenguaje _managerLenguaje;

    public ServiceBase(IMapper mapper, ILogger logger, IManagerResourceLenguaje managerLenguaje)
    {...}

    protected void LogWarning(string message)
    {
        _logger.LogWarning("Warning: " +message); <--- Implementacion de Serilog
    }

    protected void LogError(string message, Exception exception)
    {
        _logger.LogError(exception, "Error: " + message); <--- Implementacion de Serilog
    }
}
````
Log messages are managed in the LogMessage{NameModel} class, located in ``BookPro.Common/Logger``. This class contains a dictionary (Dictionary), where the keys are values of the enum Logger{NameModel}, defined in ``BookPro.Domain/{NameModel}/Logger{NameModel}.cs``. To get the message associated with a key, the GetMessage() method is used.

Implementation:
````
public class LogMessage{NameModel} : ILogMessage{NameModel}
{
    private readonly Dictionary<Logger{NameModel}, string> Message = new()
    {
        { Logger{NameModel}.{Key}, "message " },
        { Logger{NameModel}.{Key}, "message" }
    };

    public string GetMessage(Logger{NameModel} key)
    {
        return Message[key];
    }
}
````
# Troubleshooting
In this section of the documentation we will explain about the types of errors that can arise during the execution of the application and their solutions.

### ⚠ Common errors
##### ❌ Error: failed database connection
If the application does not connect to the database, check the following files:

**ApplicationDbContextFactory**
Located in `BookPro.Infrastructure/Persistence`.
Make sure that the key in `DefaultConnection` matches the one in `appsettings.json`. 

````
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationService.GetConfiguration(AppContext.BaseDirectory);

        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); 
        // en "DefaulConnection" verifica que sea igual que en appsettings.json ↑

        return new ApplicationDbContext(optionBuilder.Options);
    }
}

````

**Appsettings**
Located in ``BookPro.Common/Config``.
This is where we will verify the DefaultConnection, that it is correct for the database connection.

````
{
  "Logging": {...}
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BookAuth;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {...},
  "Encrypt": {...},
  "ResourceLenguaje": {...}
}

````
&nbsp;
##### ❌ Error: Response.resx Recovery
If the upload of the response files fails, check these files:

**Appsettings** 
Located in ``BookPro.Common/Config``.
Make sure that the ResourceLanguage section has the correct values:
````
{
  "Logging": {...},
  "AllowedHosts": "*",
  "ConnectionStrings": {...},
  "Jwt": {...},
  "Encrypt": {...},
  "ResourceLenguaje": {
    "BaseName": "Response", <-- revisar
    "Location": "BookPro.Common" <-- revisar
  }
}
````

**Program** 
Located in ``BookPro.Common/``.
Verify that the ResourcePath setting is “Resource” and that SettingsResource is initialized correctly:

````
//Configure Lenguaje
builder.Services.Configure<SettingsResource>(configuration.GetSection("ResourceLenguaje")); <--- verificar
var supportLenguaje = new[] { "en" , "es" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{...});

var culture = Thread.CurrentThread.CurrentCulture.Name;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resource"); <-- verificar
````
**ManagerResourceLenguaje**
Ubicado ``BookPro.Common/Localization``.
Asegúrate de que localizerFactory.Create() se llame con los valores correctos:
````
_localizer = localizerFactory.Create(_resource.BaseName, _resource.Location);
````

&nbsp;
##### ❌ Error:  Length of KEY and IV
If you encrypt a string, the KEY and the IV must have the correct length:

- KEY: 16, 24 o 32 bytes.
- IV: 16 bytes.

If you want to change the KEY or the IV, make sure you comply with these requirements to avoid execution errors.

This can be seen in this service and in this part of the code, which is located at ``BookPro.Aplication/Features/Token/TokenService.cs``:

````
private async Task<TokenResponseDTO> HandleRefreshTokenNull(User user)
{
    var tokenRefreshGenerate = CreateRefreshToken(user.Id);
    tokenRefreshGenerate.Token = CryptHelper.EncryptString(tokenRefreshGenerate.Token, "sZzgXzVy7TsujN65EFLkKH8MfjXDJQnvTKq/koW0rkM=", "ESsfPTP3LjTtI9IZEqrdSw=="); <--- verificar la longitud
  ...
}
````

&nbsp;
##### ❌ Error: Token generation (Access, Refresh)
If the tokens are not generated correctly, check the Jwt configuration in appsettings.json:
- Verify that the Key is exactly 32 characters long.

````
{
  "Logging": {...},
  "AllowedHosts": "*",
  "ConnectionStrings": {...},
  "Jwt": {
    "Key": "your-very-secure-and-long-key-32chars", <--- verificar que la key tenga de longitud 32
    "Issuer": "BookPro",
    "Audience": "BookClient",
    "IP": "localhost:7093"
  },
  "Encrypt": {...},
  "ResourceLenguaje": {...}
}

````
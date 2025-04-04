using Microsoft.Extensions.Configuration;

namespace Commons;

public static class ConfigurationService
{
    private static IConfiguration _configuration;

    public static IConfiguration GetConfiguration(string basePath = null) 
    {
        basePath ??= AppContext.BaseDirectory;

        var configPath = Path.Combine(basePath, "config");

        return new ConfigurationBuilder()
            .SetBasePath(configPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) 
            .Build();
    }
}
using BookPro.Infrastructure.Repositories.Appointment;

namespace BookPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<ICompaniesRepository, CompaniesRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IMotelRepository, MotelRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IServicesRepository, ServicesRepository>();

        services.AddHostedService<LogCleanupService>();

        return services;
    }
}
using BookPro.Application.Features.Appointments.Bookings.Validations;
using BookPro.Application.Features.Appointments.Companys;
using BookPro.Application.Features.Appointments.Companys.Factory;
using BookPro.Application.Features.Appointments.Companys.Interface;
using BookPro.Application.Features.Appointments.Companys.MappProfiles;
using BookPro.Application.Features.Appointments.Companys.Validations;
using BookPro.Application.Features.Authentication.Account.Interface;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
var configuration = ConfigurationService.GetConfiguration(AppContext.BaseDirectory);

//Email
builder.Services.Configure<SettingsEmail>(configuration.GetSection("Email"));

//Configure Serilog
var connectionDataBase= configuration.GetConnectionString("DefaultConnection");

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"Serilog Error: {msg}"));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Debug()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
       connectionString: connectionDataBase,
       sinkOptions: new MSSqlServerSinkOptions { 
           TableName = "Logs", 
           AutoCreateSqlTable=false, 
           SchemaName = "dbo"
       },
       restrictedToMinimumLevel: LogEventLevel.Warning
    )
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();



//Configure Lenguaje
builder.Services.Configure<SettingsResource>(configuration.GetSection("ResourceLenguaje"));
var supportLenguaje = new[] { "en", "es" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var lenguajes = supportLenguaje.Select(c => new CultureInfo(c)).ToList();
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
    options.SupportedCultures = lenguajes;
    options.SupportedUICultures = lenguajes;
});

var culture = Thread.CurrentThread.CurrentCulture.Name;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Configure Mapper
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(RegisterProfile));
builder.Services.AddAutoMapper(typeof(AddressProfile));
builder.Services.AddAutoMapper(typeof(CompanyProfile));

//Configure Jwt Authentication
builder.Services.Configure<SettingsToken>(configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.IncludeFields = true;
});

//Inyection dependency Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountValidator, AccountValidator>();
builder.Services.AddScoped<IStringTokenValidator, StringTokenValidator>();
builder.Services.AddScoped<ILoginValidator, LoginValidator>();
builder.Services.AddScoped<IRegisterValidator, RegisterValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddScoped<IStringTokenValidator, StringTokenValidator>();
builder.Services.AddScoped<IRefreshTokenValidator, RefreshTokenValidator>();
builder.Services.AddScoped<ITokenValidator, TokenValidator>();
builder.Services.AddScoped<ITokenGenerateFactory, TokenGenerateFactory>();
builder.Services.AddScoped<IManagerResourceLenguaje, ManagerResourceLenguaje>();
builder.Services.AddScoped<ILogMessageAccount, LogMessageAccount>();
builder.Services.AddScoped<ILogMessageToken, LogMessageToken>();
builder.Services.AddScoped<ILogMessageUser, LogMessageUser>();
builder.Services.AddScoped<IEmailServices, EmailServices>();

//Appointment
builder.Services.AddScoped<IAppointmentRequestValidator, AppointmentRequestValidator>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAppointmentResponseFactory, AppointmentResponseFactory>();
builder.Services.AddScoped<ILogMessageBooking, LogMessageBooking>();
builder.Services.AddScoped<ILogMessageCompany, LogMessageCompany>();
builder.Services.AddScoped<ICompanyResponseFactory, CompanyResponseFactory>();
builder.Services.AddScoped<ICompanyValidator, CompanyValidator>();
builder.Services.AddScoped<ICompanyRequestValidator, CompanyRequestValidator>();
builder.Services.AddScoped<ICompanyAddressValidator, CompanyAddressValidator>();
builder.Services.AddScoped<ICompanyServiceValidator, CompanyServiceValidator>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(configuration);

//Configure for front
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

app.UseCors("AllowVueApp");

//Configure lenguaje
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

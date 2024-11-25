using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Services;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

Log.Information("Application is starting..."); // Test log entry

builder.Host.UseSerilog();


builder.Services.AddRateLimiter(option =>
{
    option.AddFixedWindowLimiter(policyName: "StaticControllerRateLimiter", windowsOptions =>
    {
        windowsOptions.PermitLimit = 3;
        windowsOptions.Window = TimeSpan.FromSeconds(10); //For testing
    });

    option.AddFixedWindowLimiter(policyName: "AuthControllerRateLimit", windowsOptions =>
    {
        windowsOptions.PermitLimit = 10;
        windowsOptions.Window = TimeSpan.FromDays(1);
    });

    option.AddFixedWindowLimiter(policyName: "InteractionControllerRateLimit", windowsOptions =>
    {
        windowsOptions.PermitLimit = 200;
        windowsOptions.Window = TimeSpan.FromDays(1);
    });

    option.AddFixedWindowLimiter(policyName: "UpdateActionRateLimit", windowsOptions =>
    {
        windowsOptions.PermitLimit = 2;
        windowsOptions.Window = TimeSpan.FromDays(1);
    });

    option.OnRejected = async (context, CancellationToken) =>
    {
        ILogger<Program> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("Rate limit exceeded for {Path} from User {User}", context.HttpContext.Request.Path, context.HttpContext.User.Identity?.Name ?? "unknown");

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", CancellationToken);
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITicketStore, SessionStore>();

builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 6;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
})
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnValidatePrincipal = async context =>
    {
        var ticketStore = builder.Services.BuildServiceProvider().GetRequiredService<ITicketStore>();
        options.SessionStore = ticketStore;

    };
    //options.SessionStore = builder.Services.BuildServiceProvider().GetRequiredService<ITicketStore>();
    options.LoginPath = "/Auth/login";
    options.AccessDeniedPath = "/Auth/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.SlidingExpiration = true;

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = "YourAppCookie";
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();



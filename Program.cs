using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<VerseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RootValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthValidator>();

//Session Validations
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileModel>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordModel>();

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

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



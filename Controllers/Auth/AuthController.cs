using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scriptium_backend_dotnet.Models;
using System.Security.Claims;
using scriptium_backend_dotnet.Controllers.Validation;
using scriptium_backend_dotnet.DB;
using scriptium_backend_dotnet.Models.Util;
using Microsoft.AspNetCore.RateLimiting;

namespace scriptium_backend_dotnet.Controllers.AuthHandler
{

    /// <summary>
    /// This controller consists of Auth operations. Inspect the following:
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item> 
    ///         <term>URL: "register"</term>
    ///         
    ///         <c>Register([FromBody] RegisterModel model)</c>
    ///         
    ///         <description>This function operates necessary processes to register a user as well as mandatory cookie operations: Inspect <see cref="Services.SessionStore"/> </description>
    ///     </item>
    ///     
    ///     <item> 
    ///         <term>URL: "login"</term>
    ///         
    ///         <c>Login([FromBody] LoginModel model)</c>
    ///         
    ///         <description>Operates necessary processes to login and cookie operations. Inspect <see cref="Services.SessionStore"/> . </description>
    ///     </item>
    ///     
    ///     <item> 
    ///         <term>URL: "protected"</term>
    ///         
    ///         <c>ProtectedEndpoint()</c>
    ///         
    ///         <description>Test function for authorization.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    [ApiController, Route("auth"), EnableRateLimiting("AuthControllerRateLimit")]
    public class AuthController(ApplicationDBContext db, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly ILogger<AuthController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly RoleManager<Role> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        /// <summary>
        /// Register operation and necessary cookie processes. For validation model: <see cref="RegisterModel"/>
        /// </summary>
        /// <param name="model">Necessary model for registration.</param>
        /// <remarks> 
        /// By default, users should have a collection named empty string, "", this process includes this operation also.
        /// </remarks>
        /// <returns>
        /// - 200 OK if the conditions met and registration operations is successfully completed.
        /// - 400 Bad Request if the conditions did NOT meet. Returns the reasons.
        /// - 409 Conflict if the username or email of which are should be unique, conflicts with other users.
        /// </returns>
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            _logger.LogInformation($"An registration process began.");

            User UserCreated = new()
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname ?? null!,
                IsPrivate = DateTime.UtcNow

            };

            if (model.Image != null)
            {
                using var memoryStream = new MemoryStream();
                await model.Image.CopyToAsync(memoryStream);
                UserCreated.Image = memoryStream.ToArray();
            }

            var Result = await _userManager.CreateAsync(UserCreated, model.Password);

            if (Result.Succeeded)
            {
                _logger.LogInformation($"Operation completed: New User information: Identifier: {UserCreated.Id}, Username: {UserCreated.UserName}, Email: {UserCreated.Email}, Name: {UserCreated.Name} Surname: {UserCreated.Surname} CreatedAt: {UserCreated.CreatedAt}");


                Collection DefaultCollection = new() //Default collection schema. Whenever a user registered a default collection should be created. For manuel implementation: Check: ../../DB/Triggers.sql and check trigger named: trg_CreateCollectionOnUserInsert
                {
                    Name = $"{UserCreated.Name}'s collection", //Default Collection Name
                    UserId = UserCreated.Id,
                };

                UserUpdateR DefaultUpdate = new()
                {
                    UserId = UserCreated.Id,
                    Username = UserCreated.UserName,
                    Name = UserCreated.Name,
                    Surname = UserCreated.Surname,
                    Image = UserCreated.Image,
                    Gender = UserCreated.Gender,
                    Biography = UserCreated.Biography,
                    Email = UserCreated.Email

                };

                _db.Collection.Add(DefaultCollection);
                _db.UserUpdateRs.Add(DefaultUpdate);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Default collection has been created for User: [Id: {UserCreated.Id}, Username: {UserCreated.UserName}]");

                await _signInManager.SignInAsync(UserCreated, isPersistent: true);

                return Ok(new
                {
                    message = "Registration successful!",
                });


            }

            if (Result.Errors.Any(e => e.Code == "DuplicateUserName"))
            {
                _logger.LogInformation($"Registration failed: Duplicated username tried to be created.  Duplicated username {UserCreated.UserName} Email: {UserCreated.Email}");
                return Conflict(new { message = "Username already exists." });
            }

            if (Result.Errors.Any(e => e.Code == "DuplicateEmail"))
            {
                _logger.LogInformation($"Registration failed: Duplicated Email tried to be created. Duplicated Email: {UserCreated.Email} Username: {UserCreated.UserName}");
                return Conflict(new { message = "Email already exists." });
            }

            foreach (IdentityError error in Result.Errors)
                _logger.LogWarning($"Registration failed: {error.Code} - {error.Description}");

            return BadRequest(Result.Errors);
        }

        /// <summary>
        /// Login operation and necessary cookie processes. For validation model: <see cref="LoginModel"/>
        /// </summary>
        /// <param name="model">Necessary model for login operations.</param>
        /// <remarks> 
        /// This process includes "melting" the account if account is "frozen". 11.
        /// </remarks>
        /// <returns>
        /// - 200 OK if the conditions met and login operations is successfully completed.
        /// - 400 Bad Request if credentials does not match.
        /// </returns>
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            User? UserRequested = await _userManager.FindByEmailAsync(model.Email);

            if (UserRequested == null)
            {
                _logger.LogInformation($"Login failed: User with that name is not found. Claimed Email: {model.Email}");
                return Unauthorized(new { message = "Invalid Credentials!" });
            }

            var Result = await _signInManager.CheckPasswordSignInAsync(UserRequested, model.Password, lockoutOnFailure: false);

            if (!Result.Succeeded)
            {
                _logger.LogInformation($"Password is incompatible. Claimed password: {model.Password} for User: [Id: {UserRequested.Id}]");
                return Unauthorized(new { message = "Invalid Credentials!" });
            }

            if (UserRequested.IsFrozen != null)
            {

                FreezeR freezeR = new()
                {
                    UserId = UserRequested.Id,
                    Status = FreezeStatus.Unfrozen
                };

                UserRequested.IsFrozen = null;

                _db.FreezeR.Add(freezeR);

                await _db.SaveChangesAsync();
                await _userManager.UpdateAsync(UserRequested);
                _logger.LogInformation($"User had been frozen, melted, User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}]");
            }

            await _signInManager.SignInAsync(UserRequested, isPersistent: true);

            _logger.LogInformation($"User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has logged in.");

            return Ok(new
            {
                message = "Successfully logged in!",
            });
        }

        /// <summary>
        /// Test handler for authentication.
        /// </summary>
        [HttpGet("protected"), Authorize]
        public IActionResult ProtectedEndpoint()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.Identity?.Name;

            return Ok(new
            {
                message = "You are authenticated!",
                UserId = userId,
                Username = username
            });
        }
    }
}
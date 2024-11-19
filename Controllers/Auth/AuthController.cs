using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using writings_backend_dotnet.Models;
using System.Security.Claims;
using writings_backend_dotnet.Controllers.Validation;
using static writings_backend_dotnet.Controllers.Validation.AuthValidator;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(
        ApplicationDBContext db,
        UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          ILogger<AuthController> logger
                          ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly ILogger<AuthController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname ?? null!
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok(new { message = "Registration successful!" });


            if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                return BadRequest(new { message = "Username already exists." });

            if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                return BadRequest(new { message = "Email already exists." });

            return BadRequest(result.Errors);
        }


        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation($"Login attempt for user: {model.Username}");

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                _logger.LogWarning($"Login failed: User not found: {model.Username}");
                return BadRequest(new { message = "Invalid Credentials!" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Login failed: Incorrect password for user: {model.Username}");
                return BadRequest(new { message = "Invalid Credentials!" });
            }

            if (user.IsFrozen != null)
            {
                var freezeR = new FreezeR
                {
                    UserId = user.Id,
                    Status = FreezeStatus.Unfrozen
                };

                _db.FreezeR.Add(freezeR);
                await _db.SaveChangesAsync();
            }

            await _signInManager.SignInAsync(user, isPersistent: true);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation($"User: {model.Username} successfully authenticated.");
            return Ok(new { message = "Successfully logged in!" });
        }


        [HttpGet("protected"), Authorize] //This handler is just for test!
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
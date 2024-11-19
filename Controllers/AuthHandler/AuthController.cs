using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using writings_backend_dotnet.Models;
using System.Security.Claims;
using writings_backend_dotnet.Controllers.Validation;
using static writings_backend_dotnet.Controllers.Validation.AuthValidator;

namespace writings_backend_dotnet.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          ILogger<AuthController> logger
                          ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly ILogger<AuthController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


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

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                    return BadRequest(new { Message = "Username already exists." });

                if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                    return BadRequest(new { Message = "Email already exists." });

                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Registration successful!" });
        }


        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation($"Login attempt for user: {model.Username}");

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User: {model.Username} successfully authenticated.");

                    await _signInManager.SignInAsync(user, isPersistent: true);

                    return Ok(new { Message = "Successfully logged in!" });
                }
                else
                {
                    _logger.LogWarning($"Login failed: Incorrect password for user: {model.Username}");
                }
            }
            else
            {
                _logger.LogWarning($"Login failed: User not found: {model.Username}");
            }

            return BadRequest(new { message = "Invalid Credentials!" });
        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            string Username = User.Identity?.Name ?? "\\unknown Username";

            _logger.LogInformation($"User {Username} has logged out.");

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"User with username {Username} successfully logged out.");

            return Ok(new { message = "Successfully logged out" });
        }

        [HttpGet("protected"), Authorize] //This handler is just for test!
        public IActionResult ProtectedEndpoint()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.Identity?.Name;
            return Ok(new
            {
                Message = "You are authenticated!",
                UserId = userId,
                Username = username
            });
        }
    }
}
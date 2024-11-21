using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using writings_backend_dotnet.Models;
using System.Security.Claims;
using writings_backend_dotnet.Controllers.Validation;
using static writings_backend_dotnet.Controllers.Validation.AuthValidator;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.AuthHandler
{
    [ApiController]
    [Route("auth")]
    public class AuthController(ApplicationDBContext db, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
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

            var Result = await _userManager.CreateAsync(user, model.Password);

            if (Result.Succeeded)
            {
                Collection DefaultCollection = new() //Default collection schema. Whenever a user registered a default collection should be created. Check: ../../DB/Triggers.sql and check trigger named: trg_CreateCollectionOnUserInsert
                {
                    Name = "", //Default Collection Name
                    User = user,
                };

                _db.Collection.Add(DefaultCollection);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Registration successful!" });
            }

            if (Result.Errors.Any(e => e.Code == "DuplicateUserName"))
                return BadRequest(new { Message = "Username already exists." });

            if (Result.Errors.Any(e => e.Code == "DuplicateEmail"))
                return BadRequest(new { Message = "Email already exists." });

            return BadRequest(Result.Errors);
        }


        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            User? UserRequested = await _userManager.FindByNameAsync(model.Username);
            if (UserRequested == null)
                return BadRequest(new { Message = "Invalid Credentials!" });

            var Result = await _signInManager.CheckPasswordSignInAsync(UserRequested, model.Password, lockoutOnFailure: false);

            if (!Result.Succeeded)
                return BadRequest(new { Message = "Invalid Credentials!" });


            if (UserRequested.IsFrozen != null)
            {
                var freezeR = new FreezeR
                {
                    UserId = UserRequested.Id,
                    Status = FreezeStatus.Unfrozen
                };

                _db.FreezeR.Add(freezeR);
                await _db.SaveChangesAsync();
            }

            await _signInManager.SignInAsync(UserRequested, isPersistent: true);
            await _userManager.UpdateAsync(UserRequested);

            return Ok(new { Message = "Successfully logged in!" });
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
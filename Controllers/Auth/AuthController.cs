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
    [ApiController, Route("auth")]
    public class AuthController(ApplicationDBContext db, UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));


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
        /// </returns>
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
                Message = "You are authenticated!",
                UserId = userId,
                Username = username
            });
        }
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.SessionHandler
{
    [ApiController, Route("session"), Authorize]
    public class SessionController(ApplicationDBContext db, ILogger<SessionController> logger, SignInManager<User> signInManager, UserManager<User> userManager) : ControllerBase
    {
        private readonly ILogger<SessionController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));

        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout()
        {
            string Username = User.Identity?.Name ?? "\\unknown Username";

            _logger.LogInformation($"User {Username} has logged out.");

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"User with username {Username} successfully logged out.");

            return Ok(new { message = "Successfully logged out" });
        }

        [HttpPost, Route("alter")]
        public async Task<IActionResult> Alter()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Something went wrong!" });

            bool IsPrivate = user.IsPrivate.HasValue;

            user.IsPrivate = IsPrivate ? null : DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            string message = $"You account is now {(IsPrivate ? "public" : "private")}!";

            return Ok(new { message });
        }

        [HttpPost, Route("freeze")]
        public async Task<IActionResult> Freeze()
        {
            FreezeR? freezeRecord;

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "Something went wrong!" });

            freezeRecord = await _db.FreezeR.OrderByDescending(r => r.ProceedAt)
                                            .FirstOrDefaultAsync(r => r.UserId.ToString() == userId && r.Status == FreezeStatus.Frozen && r.ProceedAt < DateTime.UtcNow.AddDays(7));

            if (freezeRecord != null)
                return Unauthorized(new { message = "You cannot freeze your account twice within 7 days!" });

            freezeRecord = new FreezeR
            {
                UserId = user.Id,
                Status = FreezeStatus.Frozen,
                ProceedAt = DateTime.UtcNow,
            };

            user.IsFrozen = DateTime.UtcNow;

            _db.FreezeR.Add(freezeRecord);
            await _db.SaveChangesAsync();

            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();

            string message = "Your account has been successfully frozen!";
            return Ok(new { message });
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {

            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);

                if (existingUser != null)
                    return BadRequest(new { message = "Username is already taken!" });

            }

            if (model.LanguageId.HasValue)
            {
                var languageExists = await _db.Language
                    .AnyAsync(l => l.Id == model.LanguageId);

                if (!languageExists)
                    return BadRequest(new { message = "Invalid language ID!" });

            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found!" });

            if (!string.IsNullOrWhiteSpace(model.Name))
                user.Name = model.Name.Trim();

            if (!string.IsNullOrWhiteSpace(model.Surname))
                user.Surname = model.Surname.Trim();

            if (!string.IsNullOrWhiteSpace(model.Username))
                user.UserName = model.Username.Trim();

            if (!string.IsNullOrWhiteSpace(model.Biography))
                user.Biography = model.Biography.Trim();

            if (!string.IsNullOrWhiteSpace(model.Gender))
                user.Gender = model.Gender.Trim();

            if (model.LanguageId.HasValue)
                user.PreferredLanguageId = model.LanguageId.Value;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return BadRequest(new { message = "Failed to update profile!" });

            return Ok(new { message = "Profile updated successfully!" });
        }

        [HttpPut, Route("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found!" });

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!passwordCheck)
                return BadRequest(new { message = "The old password is incorrect." });

            if (model.OldPassword == model.NewPassword)
                return BadRequest(new { message = "The new password cannot be the same as the old password." });

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new
                {
                    message = "Failed to change password.",
                    errors = result.Errors.Select(e => e.Description)
                });


            return Ok(new { message = "Password changed successfully!" });
        }

    }
}
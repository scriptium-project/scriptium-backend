using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.SessionHandler
{
    [ApiController, Route("session"), Authorize, EnableRateLimiting(policyName: "InteractionControllerRateLimit")]
    public class SessionController(ApplicationDBContext db, ILogger<SessionController> logger, SignInManager<User> signInManager, UserManager<User> userManager) : ControllerBase
    {
        private readonly ILogger<SessionController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));

        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { message = "Something went wrong!" });

            _logger.LogInformation($"User {UserRequested.UserName} has tried to log out.");

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] successfully logged out.");

            return Ok(new { message = "Successfully logged out" });
        }

        [HttpPost, Route("alter")]
        public async Task<IActionResult> Alter()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { message = "Something went wrong!" });

            bool IsPrivate = UserRequested.IsPrivate.HasValue;

            UserRequested.IsPrivate = IsPrivate ? null : DateTime.UtcNow;
            try
            {
                string AccountStatus = IsPrivate ? "public" : "private";

                await _userManager.UpdateAsync(UserRequested);
                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id} Username: {UserRequested.UserName}] has changed his account to {AccountStatus}");
                return Ok(new { message = $"You account is now {AccountStatus}!" });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id} Username: {UserRequested.UserName}] alter his account. Error Detail: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpPost, Route("freeze"), EnableRateLimiting(policyName: "UpdateActionRateLimit")]
        public async Task<IActionResult> Freeze()
        {
            FreezeR? FreezeRecord;

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(userId);

            if (UserRequested == null)
                return NotFound(new { message = "Something went wrong!" });
            try
            {
                FreezeRecord = await _db.FreezeR.OrderByDescending(r => r.ProceedAt)
                                                .FirstOrDefaultAsync(r => r.UserId.ToString() == userId && r.Status == FreezeStatus.Frozen && r.ProceedAt < DateTime.UtcNow.AddDays(7));

                if (FreezeRecord != null)
                {
                    _logger.LogWarning($"Conflict occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to freeze his account twice within 7 days.");
                    return Unauthorized(new { message = "You cannot freeze your account twice within 7 days!" });
                }

                FreezeRecord = new FreezeR
                {
                    UserId = UserRequested.Id,
                    Status = FreezeStatus.Frozen,
                    ProceedAt = DateTime.UtcNow,
                };

                UserRequested.IsFrozen = DateTime.UtcNow;

                _db.FreezeR.Add(FreezeRecord);

                List<Session> ActiveSessions = await _db.Session.Where(s => s.UserId == UserRequested.Id).ToListAsync();

                foreach (Session session in ActiveSessions)
                    _db.Session.Remove(session);

                await _db.SaveChangesAsync();

                await _userManager.UpdateAsync(UserRequested);
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has frozen his account.");
                return Ok(new { message = "Your account has been successfully frozen!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] trying to freeze his account. Error Details: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }


        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {


            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(userId);

            if (UserRequested == null)
                return NotFound(new { message = "UserRequested not found!" });

            string UpdateLogRow = "";

            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);

                if (existingUser != null)
                    return BadRequest(new { message = "Username is already taken!" });
            }

            if (model.LanguageId.HasValue)
            {
                bool languageExists = await _db.Language
                    .AnyAsync(l => l.Id == model.LanguageId);

                if (!languageExists)
                    return BadRequest(new { message = "Invalid language ID!" });
            }

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                UpdateLogRow += $" Name: {UserRequested.Name} -> {model.Name.Trim()}";
                UserRequested.Name = model.Name.Trim();
            }

            if (!string.IsNullOrWhiteSpace(model.Surname))
            {
                UpdateLogRow += $" Surname: {UserRequested.Surname} -> {model.Surname.Trim()}";
                UserRequested.Surname = model.Surname.Trim();
            }

            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                UpdateLogRow += $" UserName: {UserRequested.UserName} -> {model.Username.Trim()}";
                UserRequested.UserName = model.Username.Trim();
            }

            if (!string.IsNullOrWhiteSpace(model.Biography))
            {
                UpdateLogRow += $" Biography: {UserRequested.Biography} -> {model.Biography.Trim()}";
                UserRequested.Biography = model.Biography.Trim();
            }

            if (!string.IsNullOrWhiteSpace(model.Gender))
            {
                UpdateLogRow += $" Gender: {UserRequested.Gender} -> {model.Gender.Trim()}";
                UserRequested.Gender = model.Gender.Trim();
            }

            if (model.LanguageId.HasValue)
            {
                UpdateLogRow += $" Preferred LanguageId: {UserRequested.PreferredLanguageId} -> {model.LanguageId.Value}";
                UserRequested.PreferredLanguageId = model.LanguageId.Value;
            }

            var updateResult = await _userManager.UpdateAsync(UserRequested);

            if (!updateResult.Succeeded)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is update his profile: {UpdateLogRow}. Error Details: {updateResult.Errors}");
                return BadRequest(new
                {
                    message = "Failed to update profile!",
                    errors = updateResult.Errors.Select(e => e.Description)
                });
            }

            _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has updated his account: {UpdateLogRow}");
            return Ok(new { message = "Profile updated successfully!" });
        }

        [HttpPut, Route("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(userId);

            if (UserRequested == null)
                return NotFound(new { message = "User not found!" });

            var passwordCheck = await _userManager.CheckPasswordAsync(UserRequested, model.OldPassword);

            if (!passwordCheck)
                return BadRequest(new { message = "The old password is incorrect." });

            if (model.OldPassword == model.NewPassword)
                return BadRequest(new { message = "The new password cannot be the same as the old password." });

            var result = await _userManager.ChangePasswordAsync(UserRequested, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is update his password. Error Details: {result.Errors}");

                return BadRequest(new
                {
                    message = "Failed to change password.",
                    errors = result.Errors.Select(e => e.Description)
                });
            }
            _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has updated his password.");
            return Ok(new { message = "Password changed successfully!" });
        }

    }
}
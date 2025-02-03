using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using scriptium_backend_dotnet.DB;
using scriptium_backend_dotnet.DTOs;
using scriptium_backend_dotnet.Models;
using scriptium_backend_dotnet.Models.Util;

namespace scriptium_backend_dotnet.Controllers.UserHandler
{
    [ApiController, Route("user"), EnableRateLimiting(policyName: "InteractionControllerRateLimit"), Authorize]
    public class UserController(UserManager<User> userManager, ApplicationDBContext db) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));


        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { message = "User not found." });

            User? UserFetched = await _userManager.FindByNameAsync(username);

            if (UserFetched == null || UserFetched.IsFrozen.HasValue)
                return NotFound(new { message = "User not found!" });


            bool IsUserInspectingBlocked = await _db.Block.AnyAsync(b => b.BlockerId == UserFetched.Id && b.BlockedId == UserRequested.Id);

            if (IsUserInspectingBlocked)
                return NotFound(new { message = "User not found!" });

            var roles = (await _userManager.GetRolesAsync(UserFetched)).ToList();

            long followerCount = await _db.Follow.CountAsync(r => r.FollowedId == UserFetched.Id && !r.Follower.IsFrozen.HasValue && r.Status == FollowStatus.Accepted);
            long followingCount = await _db.Follow.CountAsync(r => r.FollowerId == UserFetched.Id && !r.Followed.IsFrozen.HasValue && r.Status == FollowStatus.Accepted);
            long commentCount = await _db.Comment.CountAsync(c => c.UserId == UserFetched.Id);
            long noteCount = await _db.Note.CountAsync(n => n.UserId == UserFetched.Id);
            long suggestionCount = await _db.Suggestion.CountAsync(s => s.UserId == UserFetched.Id);

            Dictionary<string, object> data = new()
            {
                { "id", UserFetched.Id },
                { "username", UserFetched.UserName ?? string.Empty },
                { "name", UserFetched.Name ?? string.Empty },
                { "surname", UserFetched.Surname ?? string.Empty },
                { "biography", UserFetched.Biography ?? string.Empty },
                { "image", UserFetched.Image != null ? Convert.ToBase64String(UserFetched.Image) : null! },
                { "followerCount", followerCount },
                { "followingCount", followingCount },
                { "reflectionCount", commentCount },
                { "noteCount", noteCount },
                { "suggestionCount", suggestionCount },
                { "privateFrom", UserFetched.IsPrivate?.ToString("yyyy-MM-dd")! },
                { "roles", roles },
                { "createdAt", UserFetched.CreatedAt.ToString("yyyy-MM-dd") },
                { "updateCount", UserFetched.UpdateCount}
            };


            Follow? FollowStatusFromUserInspectingToUserInspected = await _db.Follow.FirstOrDefaultAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == UserFetched.Id);

            if (FollowStatusFromUserInspectingToUserInspected != null)
                data["followStatusUserInspecting"] = FollowStatusFromUserInspectingToUserInspected.Status.ToString();

            Follow? FollowStatusFromUserInspectedToUserInspecting = await _db.Follow.FirstOrDefaultAsync(f => f.FollowedId == UserRequested.Id && f.FollowerId == UserFetched.Id);

            if (FollowStatusFromUserInspectedToUserInspecting != null)
                data["followStatusUserInspected"] = FollowStatusFromUserInspectedToUserInspecting.Status.ToString();

            bool IsUserInspectedBlocked = await _db.Block.AnyAsync(b => b.BlockerId == UserRequested.Id && b.BlockedId == UserFetched.Id);

            if (IsUserInspectedBlocked)
                data["isUserInspectedBlocked"] = true;

            return Ok(new { data });
        }

        [HttpGet("comments/{userId}")]
        public async Task<IActionResult> GetUsersComments([FromRoute(Name = "userId")] string userFetchedUserId)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { message = "User not found." });


            User? UserFetched = await _userManager.FindByIdAsync(userFetchedUserId);

            if (UserFetched == null || UserFetched.IsFrozen.HasValue)
                return NotFound(new { message = "User not found!" });


            try
            {
                bool IsUserInspectingBlocked = await _db.Block.AnyAsync(b => b.BlockerId == UserFetched.Id && b.BlockedId == UserRequested.Id);

                if (IsUserInspectingBlocked)
                    return NotFound(new { message = "User not found!" });


                List<CommentDTOExtended>? data = await _db.Comment
                    .Where(c => c.UserId == UserFetched.Id && c.CommentVerse != null && c.LikeComments != null)
                    .Include(c => c.CommentVerse).ThenInclude(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Scripture).ThenInclude(s => s.Meanings).ThenInclude(m => m.Language)
                    .Include(c => c.CommentVerse).ThenInclude(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Meanings).ThenInclude(m => m.Language)
                    .Include(c => c.CommentVerse).ThenInclude(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Meanings).ThenInclude(m => m.Language)
                    .AsSplitQuery()
                   .Select(comment => comment.ToCommentDTOExtended(UserRequested))
                   .ToListAsync();


                return Ok(new { data });
            }
            catch (Exception ex)
            {


                return BadRequest(new { message = "Something went unexpectedly wrong?" });

            }

        }

        [HttpGet("notes/{userId}")]
        public async Task<IActionResult> GetUsersNotes([FromRoute(Name = "userId")] string userFetchedUserId)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { message = "User not found." });


            User? UserFetched = await _userManager.FindByIdAsync(userFetchedUserId);

            if (UserFetched == null || UserFetched.IsFrozen.HasValue)
                return NotFound(new { message = "User not found!" });


            try
            {
                bool IsUserInspectingBlocked = await _db.Block.AnyAsync(b => b.BlockerId == UserFetched.Id && b.BlockedId == UserRequested.Id);

                if (IsUserInspectingBlocked)
                    return NotFound(new { message = "User not found!" });


                List<NoteDTOExtended>? data = await _db.Note
                    .Where(n => n.UserId == UserFetched.Id && n.Likes != null)
                    .Include(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Scripture).ThenInclude(s => s.Meanings).ThenInclude(m => m.Language)
                    .Include(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Section).ThenInclude(s => s.Meanings).ThenInclude(m => m.Language)
                    .Include(cv => cv.Verse).ThenInclude(v => v.Chapter).ThenInclude(c => c.Meanings).ThenInclude(m => m.Language)
                    .AsSplitQuery()
                   .Select(note => note.ToNoteDTOExtended(UserRequested))
                   .ToListAsync();


                return Ok(new { data });
            }
            catch (Exception ex)
            {


                return BadRequest(new { message = "Something went unexpectedly wrong?" });

            }




        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized(new { message = "You are not logged in!" });

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null || UserRequested.IsFrozen.HasValue)
                return NotFound(new { message = "User not found!" });

            var roles = (await _userManager.GetRolesAsync(UserRequested)).ToList();

            long followerCount = await _db.Follow.CountAsync(r => r.FollowedId == UserRequested.Id && !r.Follower.IsFrozen.HasValue && r.Status == FollowStatus.Accepted);
            long followingCount = await _db.Follow.CountAsync(r => r.FollowerId == UserRequested.Id && !r.Followed.IsFrozen.HasValue && r.Status == FollowStatus.Accepted);
            long commentCount = await _db.Comment.CountAsync(c => c.UserId == UserRequested.Id);
            long noteCount = await _db.Note.CountAsync(n => n.UserId == UserRequested.Id);
            long suggestionCount = await _db.Suggestion.CountAsync(s => s.UserId == UserRequested.Id);

            Dictionary<string, object> data = new()
            {
                { "id", UserRequested.Id },
                { "username", UserRequested.UserName ?? string.Empty },
                { "name", UserRequested.Name ?? string.Empty },
                { "surname", UserRequested.Surname ?? string.Empty },
                { "gender", UserRequested.Gender },
                { "email", UserRequested.Email },
                { "langId", UserRequested.PreferredLanguageId},
                { "biography", UserRequested.Biography ?? string.Empty },
                { "image", UserRequested.Image != null ? Convert.ToBase64String(UserRequested.Image) : null! },
                { "privateFrom", UserRequested.IsPrivate },
                { "roles", roles },
                { "createdAt", UserRequested.CreatedAt.ToString("yyyy-MM-dd") }

            };

            return Ok(new { data });
        }

    }

}
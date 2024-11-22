using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.FollowHandler
{

    [ApiController, Route("follow"), Authorize]
    public class FollowController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        [HttpGet("follower/{type}")]
        public async Task<IActionResult> GetFollower([FromRoute] FollowStatus type)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });


            try
            {
                var data = await _db.Follow
                    .Where(f => f.FollowedId == UserRequested.Id && f.Status == type)
                    .Select(f => new { f.Follower.UserName, f.OccurredAt, IsFrozen = f.Follower.IsFrozen.HasValue })
                    .ToListAsync();

                return Ok(new { data });
            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpGet, Route("followed/{type}")]
        public async Task<IActionResult> GetFollowed([FromRoute] FollowStatus type)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();


            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound();

            try
            {
                var data = await _db.Follow.OrderByDescending(f => f.OccurredAt).Where(f => f.FollowerId == UserRequested.Id && f.Status == type).Select(f => new { f.Followed.UserName, f.OccurredAt, IsFrozen = f.Followed.IsFrozen.HasValue }).ToListAsync();
                return Ok(new { data });
            }
            catch
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpPost, Route("follow")]
        public async Task<IActionResult> Follow([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserFollowed = await _userManager.FindByNameAsync(model.UserName);

            bool isBlocked;

            if (UserRequested == null || UserFollowed == null || UserFollowed.IsFrozen.HasValue || (isBlocked = await _db.Block.AnyAsync(b => b.BlockedId == UserFollowed.Id && b.BlockerId == UserRequested.Id)))
                return NotFound(new { Message = "User not found." });

            if (UserRequested.Id == UserFollowed.Id)
                return BadRequest(new { Message = "You cannot follow yourself." });

            bool isAlreadyFollowing = await _db.Follow.AnyAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == UserFollowed.Id);

            if (isAlreadyFollowing)
                return Ok(new { Message = "You are already following this user or a follow request is pending." });


            bool IsUserFollowedPrivate = UserFollowed.IsPrivate.HasValue;

            Follow follow = new()
            {
                Follower = UserRequested,
                Followed = UserFollowed,
                Status = IsUserFollowedPrivate ? FollowStatus.Pending : FollowStatus.Accepted
            };

            try
            {

                _db.Follow.Add(follow);
                await _db.SaveChangesAsync();

                string Message = IsUserFollowedPrivate ? "Follow request sent successfully." : "You are successfully following the user!";

                return Ok(new { Message });
            }
            catch
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }


        [HttpPut, Route("accept")]
        public async Task<IActionResult> Accept([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserAccepted = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserAccepted == null || UserAccepted.IsFrozen.HasValue)
                return NotFound(new { Message = "User not found." });

            try
            {
                Follow? Follow = await _db.Follow.OrderByDescending(f => f.OccurredAt).FirstOrDefaultAsync(f => f.FollowedId == UserRequested.Id && f.FollowerId == UserAccepted.Id && f.Status == FollowStatus.Pending);


                if (Follow == null) return BadRequest(new { Message = "Follow request either is already accepted or retrieved from the user!" });

                Follow.OccurredAt = DateTime.Now;
                Follow.Status = FollowStatus.Accepted;

                _db.Follow.Update(Follow);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Follow request is successfully accepted!" });

            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpDelete, Route("reject")]
        public async Task<IActionResult> Reject([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserRejected = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserRejected == null || UserRejected.IsFrozen.HasValue)
                return NotFound(new { Message = "User not found." });

            try
            {
                Follow? Follow = await _db.Follow.OrderByDescending(f => f.OccurredAt).FirstOrDefaultAsync(f => f.FollowedId == UserRequested.Id && f.FollowerId == UserRejected.Id && f.Status == FollowStatus.Pending);

                if (Follow == null) return BadRequest(new { Message = "Follow request either is already accepted or retrieved from the user!" });

                _db.Follow.Remove(Follow);
                await _db.SaveChangesAsync();


                return Ok(new { Message = "The request is successfully rejected!" });
            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpDelete, Route("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserUnfollowed = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserUnfollowed == null || UserUnfollowed.IsFrozen.HasValue)
                return NotFound(new { Message = "User not found." });

            try
            {
                Follow? Follow = await _db.Follow.OrderByDescending(f => f.OccurredAt).FirstOrDefaultAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == UserUnfollowed.Id && f.Status == FollowStatus.Accepted);

                if (Follow == null) return BadRequest(new { Message = "You are already not following the user!" });

                _db.Follow.Remove(Follow);
                await _db.SaveChangesAsync();


                return Ok(new { Message = "The user is successfully unfollowed!" });

            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpDelete, Route("remove")]
        public async Task<IActionResult> Remove([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserRemoved = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserRemoved == null || UserRemoved.IsFrozen.HasValue)
                return NotFound(new { Message = "User not found." });

            try
            {
                Follow? Follow = await _db.Follow.OrderByDescending(f => f.OccurredAt).FirstOrDefaultAsync(f => f.FollowedId == UserRequested.Id && f.FollowerId == UserRemoved.Id && f.Status == FollowStatus.Accepted);


                if (Follow == null) return BadRequest(new { Message = "The user is not following you!" });

                _db.Follow.Remove(Follow);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have successfully removed the user!" });

            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpDelete, Route("retrieve")]
        public async Task<IActionResult> Retrieve([FromBody] UserNameModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserRetrieved = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserRetrieved == null || UserRetrieved.IsFrozen.HasValue)
                return NotFound(new { Message = "User not found." });

            try
            {
                Follow? Follow = await _db.Follow.OrderByDescending(f => f.OccurredAt).FirstOrDefaultAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == UserRetrieved.Id && f.Status == FollowStatus.Pending);

                if (Follow == null) return BadRequest(new { Message = "You do not have pending request for this user!" });

                _db.Follow.Remove(Follow);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "The follow request is successfully retrieved!" });

            }
            catch
            {
                return StatusCode(500, new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}
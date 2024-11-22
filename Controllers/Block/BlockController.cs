using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.BlockHandler
{
    [ApiController, Route("block"), Authorize]
    public class BlockController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        [HttpGet, Route("")]
        public async Task<IActionResult> GetBlocked()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            var data = await _db.Block.Where(b => b.BlockerId == UserRequested.Id).Select(b => new { b.Reason, b.Blocked.UserName, b.BlockedAt }).ToListAsync();

            return Ok(new { data });
        }

        [HttpPost, Route("block")]
        public async Task<IActionResult> Block([FromBody] BlockModel model)
        {

            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserBlocked = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserBlocked == null)
                return NotFound(new { Message = "User not found." });

            if (UserRequested.Id == UserBlocked.Id)
                return BadRequest(new { Message = "You cannot block yourself." });

            bool isAlreadyBlocked = await _db.Block.AnyAsync(f => f.BlockerId == UserRequested.Id && f.BlockedId == UserBlocked.Id);

            if (isAlreadyBlocked)
                return Ok(new { Message = "You already did NOT blocked this user." });

            Block block = new()
            {
                BlockerId = UserRequested.Id,
                BlockedId = UserBlocked.Id,
                Reason = model.Reason
            };


            using IDbContextTransaction transaction = await _db.Database.BeginTransactionAsync();

            try
            {

                //Entities should be deleted whenever a user blocks another:
                List<Comment> CommentsToBeDeleted = await _db.Comment.Where(c => c.User.Id == UserBlocked.Id && c.ParentComment != null && c.ParentComment.User.Id == UserRequested.Id).ToListAsync();
                List<Like> LikesToBeDeleted = await _db.Like.Where(l => l.UserId == UserBlocked.Id && ((l.LikeNote != null && l.LikeNote.Note.UserId == UserRequested.Id) || (l.LikeComment != null && l.LikeComment.Comment.UserId == UserRequested.Id))).ToListAsync();
                List<Notification> NotificationsToBeDeleted = await _db.Notification.Where(n => n.ActorId == UserRequested.Id && n.RecipientId == UserBlocked.Id).ToListAsync();
                List<Follow> FollowRecordsToBeDeleted = await _db.Follow.Where(f => (f.FollowerId == UserRequested.Id && f.FollowedId == UserBlocked.Id) || (f.FollowerId == UserBlocked.Id && f.FollowedId == UserRequested.Id)).ToListAsync();

                _db.Comment.RemoveRange(CommentsToBeDeleted);
                _db.Like.RemoveRange(LikesToBeDeleted);
                _db.Notification.RemoveRange(NotificationsToBeDeleted);
                _db.Follow.RemoveRange(FollowRecordsToBeDeleted);

                _db.Block.Add(block);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Message = "User has been successfully blocked!" });
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpDelete, Route("unblock")]
        public async Task<IActionResult> Unblock([FromBody] UserNameModel model)
        {

            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);
            User? UserBlocked = await _userManager.FindByNameAsync(model.UserName);

            if (UserRequested == null || UserBlocked == null)
                return NotFound(new { Message = "User not found." });

            if (UserRequested.Id == UserBlocked.Id)
                return BadRequest(new { Message = "You cannot block yourself." });

            Block? BlockRecord = await _db.Block.FirstOrDefaultAsync(f => f.BlockerId == UserRequested.Id && f.BlockedId == UserBlocked.Id);

            if (BlockRecord == null)
                return Ok(new { Message = "You already did NOT blocked this user." });



            try
            {
                _db.Block.Remove(BlockRecord);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "User block has been successfully removed!" });
            }
            catch
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

    }
}
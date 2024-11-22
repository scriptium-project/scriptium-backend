using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.LikeHandler
{
    [ApiController, Route("follow"), Authorize]
    public class LikeController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpPost, Route("like/note")]
        public async Task<IActionResult> LikeNote([FromBody] LikeNoteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Note? NoteLiked = await _db.Note.FirstOrDefaultAsync(n => n.Id == model.NoteId);

            if (NoteLiked == null)
                return NotFound(new { Message = "Note not found." });

            bool isFollowingOwner = await _db.Follow.AnyAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == NoteLiked.UserId && f.Status == FollowStatus.Accepted);

            if (!isFollowingOwner)
                return Unauthorized(new { Message = "You do not have permission do like this note." });


            Like LikeCreated = new()
            {
                UserId = UserRequested.Id,

            };

            LikeNote LikeNoteCreated = new()
            {
                LikeId = LikeCreated.Id,
                NoteId = NoteLiked.Id
            };

            _db.Like.Add(LikeCreated);
            _db.LikeNote.Add(LikeNoteCreated);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Note is successfully liked" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPost, Route("like/comment")]
        public IActionResult LikeComment([FromBody] LikeCommentModel model)
        {
            //TODO: Will be implemented.
            return Ok();
        }

        [HttpDelete, Route("unlike/note")]
        public async Task<IActionResult> UnlikeNote([FromBody] LikeNoteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Like? LikeDeleted = await _db.Like.FirstOrDefaultAsync(l => l.UserId == UserRequested.Id && l.LikeNote != null && l.LikeNote.NoteId == model.NoteId);

            if (LikeDeleted == null)
                return NotFound(new { Message = "There is no 'like' attached this note." });


            _db.Like.Remove(LikeDeleted);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Like that attached this note is successfully deleted." });
            }
            catch (Exception)
            {

                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpDelete, Route("unlike/comment")]
        public async Task<IActionResult> UnlikeVerse([FromBody] LikeCommentModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Like? LikeDeleted = await _db.Like.FirstOrDefaultAsync(l => l.UserId == UserRequested.Id && l.LikeComment != null && l.LikeComment.CommentId == model.CommentId);

            if (LikeDeleted == null)
                return NotFound(new { Message = "There is no 'like' attached this comment." });

            _db.Like.Remove(LikeDeleted);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Like that attached this comment is successfully deleted." });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}
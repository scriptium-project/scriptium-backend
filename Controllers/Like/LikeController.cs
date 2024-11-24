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
    [ApiController, Route("like"), Authorize]
    public class LikeController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpPost, Route("note")]
        public async Task<IActionResult> LikeNote([FromBody] NoteIdentifierModel model)
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

        [HttpPost, Route("comment/note")]
        public async Task<IActionResult> LikeCommentOnNote([FromBody] CommentLikeProcessModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });


            Note? NoteTarget = await _db.Note.FirstOrDefaultAsync(n => n.Id == model.EntityId);

            if (NoteTarget == null)
                return NotFound(new { Message = "Note not found." });

            Comment? CommentLiked = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.CommentNote != null && c.CommentNote.NoteId == NoteTarget.Id);

            if (CommentLiked == null)
                return NotFound(new { Message = "Comment not found." });


            HashSet<long> LikeableNoteCommentIds = _db.GetAvailableNoteCommentIds(UserRequested.Id, NoteTarget.Id);

            bool isFollowing = await _db.Follow.AnyAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == NoteTarget.UserId && f.Status == FollowStatus.Accepted);

            if (!(isFollowing && LikeableNoteCommentIds.Contains(CommentLiked.Id)))
                return Unauthorized(new { Message = "You do not have permission to attach comment to this note" });

            Like LikeCreated = new()
            {
                UserId = UserRequested.Id,
            };

            LikeNote LikeNoteCreated = new()
            {
                NoteId = NoteTarget.Id,
                LikeId = LikeCreated.Id,
            };
            _db.Like.Add(LikeCreated);
            _db.LikeNote.Add(LikeNoteCreated);
            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have successfully liked the comment on this specified note!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });

            }

        }

        [HttpPost, Route("comment/verse")]
        public async Task<IActionResult> LikeCommentVerse([FromBody] CommentLikeProcessModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Verse? VerseTarget = await _db.Verse.FirstOrDefaultAsync(n => n.Id == model.EntityId);

            if (VerseTarget == null)
                return NotFound(new { Message = "Verse not found." });

            Comment? CommentLiked = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.CommentVerse != null && c.CommentVerse.VerseId == VerseTarget.Id);

            if (CommentLiked == null)
                return NotFound(new { Message = "Comment not found." });

            HashSet<long> LikeableVerseCommentIds = _db.GetAvailableVerseCommentIds(UserRequested.Id, VerseTarget.Id);

            if (!LikeableVerseCommentIds.Contains(CommentLiked.Id))
                return Unauthorized(new { Message = "You do not have permission to attach comment to this note" });

            Like LikeCreated = new()
            {
                UserId = UserRequested.Id,
            };

            LikeComment LikeCommentCreated = new()
            {
                CommentId = CommentLiked.Id,
                LikeId = LikeCreated.Id,
            };

            _db.Like.Add(LikeCreated);
            _db.LikeComment.Add(LikeCommentCreated);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have successfully liked the comment!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });

            }

        }


        [HttpDelete, Route("unlike/note")]
        public async Task<IActionResult> UnlikeNote([FromBody] NoteIdentifierModel model)
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
        public async Task<IActionResult> UnlikeVerse([FromBody] CommentLikeProcessModel model)
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
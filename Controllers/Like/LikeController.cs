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
    //TODO: Amend
    [ApiController, Route("like"), Authorize]
    public class LikeController(ApplicationDBContext db, UserManager<User> userManager, ILogger<LikeController> logger) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ILogger<LikeController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        [HttpPost, Route("note")]
        public async Task<IActionResult> LikeNote([FromBody] NoteIdentifierModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Note? NoteLiked = null;

            try
            {
                NoteLiked = await _db.Note.FirstOrDefaultAsync(n => n.Id == model.NoteId && n.Likes != null && n.Likes.Any(l => l.Like.UserId != UserRequested.Id));

                if (NoteLiked == null)
                {
                    _logger.LogWarning($"NotFound, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to Like Claimed Note: [model.NoteId: {model.NoteId}]");
                    return NotFound(new { Message = "Note not found." });
                }

                bool isFollowingOwner = await _db.Follow.AnyAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == NoteLiked.UserId && f.Status == FollowStatus.Accepted);

                if (!isFollowingOwner)
                {
                    _logger.LogWarning($"Unauthorized, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Note: [Id: {NoteLiked.Id}, NoteOwnerUsername: {NoteLiked.User.UserName}]");
                    return Unauthorized(new { Message = "You do not have permission do like this note." });
                }


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

                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has successfully liked Note: [Id: {NoteLiked.Id}, NoteOwnerUsername: {NoteLiked.User.UserName}]");
                return Ok(new { Message = "Note is successfully liked" });
            }
            catch (Exception)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Note: [Id: {NoteLiked?.Id}, NoteOwnerUsername: {NoteLiked?.User.UserName}]");
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


            Note? NoteTarget = null;

            Comment? CommentLiked = null;

            try
            {
                NoteTarget = await _db.Note.FirstOrDefaultAsync(n => n.Id == model.EntityId);

                if (NoteTarget == null)
                {
                    _logger.LogWarning($"NotFound, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to Like Comment On Claimed Note: [model.EntityId: {model.EntityId}]");
                    return NotFound(new { Message = "Note not found." });
                }

                CommentLiked = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.CommentNote != null && c.CommentNote.NoteId == NoteTarget.Id);

                if (CommentLiked == null)
                {
                    _logger.LogWarning($"Not Found, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Claimed Comment: [model.CommentId: {model.CommentId}] on Comment/Note");
                    return NotFound(new { Message = "Comment not found." });
                }

                HashSet<long> LikeableNoteCommentIds = _db.GetAvailableNoteCommentIds(UserRequested.Id, NoteTarget.Id);

                bool isFollowing = await _db.Follow.AnyAsync(f => f.FollowerId == UserRequested.Id && f.FollowedId == NoteTarget.UserId && f.Status == FollowStatus.Accepted);

                if (!(isFollowing && LikeableNoteCommentIds.Contains(CommentLiked.Id)))
                {
                    _logger.LogWarning($"Unauthorized, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Comment: [Id: {CommentLiked.Id}, CommentOwnerUsername: {CommentLiked.User.UserName}] on Note: [Id: {NoteTarget.Id}, NoteOwnerUsername: {NoteTarget.User.UserName}]. User does not have permission to like.");
                    return Unauthorized(new { Message = "You do not have permission to attach comment to this note" });
                }

                Like LikeCreated = new()
                {
                    UserId = UserRequested.Id,
                };

                LikeComment LikeNoteCreated = new()
                {
                    LikeId = LikeCreated.Id,
                    CommentId = CommentLiked.Id
                };

                _db.Like.Add(LikeCreated);

                _db.LikeComment.Add(LikeNoteCreated);

                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed.  User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has successfully liked Comment: [Id: {CommentLiked.Id}, CommentOwnerUsername: {CommentLiked.User.UserName}] on Note: [Id: {NoteTarget.Id}, NoteOwnerUsername: {NoteTarget.User.UserName}]");
                return Ok(new { Message = "You have successfully liked the comment on this specified note!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Comment: [Id: {CommentLiked?.Id}, CommentOwnerUsername: {CommentLiked?.User.UserName}] on Note: [Id: {NoteTarget?.Id}, NoteOwnerUsername: {NoteTarget?.User.UserName}]. Error Details: {ex}");
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

            Verse? VerseTarget = null;

            Comment? CommentLiked = null;

            try
            {
                VerseTarget = await _db.Verse.FirstOrDefaultAsync(n => n.Id == model.EntityId);

                if (VerseTarget == null)
                    return NotFound(new { Message = "Verse not found." });

                CommentLiked = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.CommentVerse != null && c.CommentVerse.VerseId == VerseTarget.Id);

                if (CommentLiked == null)
                {
                    _logger.LogWarning($"Not Found, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Claimed Comment: [model.CommentId: {model.CommentId}] on Verse: [Id: {VerseTarget.Id}]");
                    return NotFound(new { Message = "Comment not found." });
                }

                HashSet<long> LikeableVerseCommentIds = _db.GetAvailableVerseCommentIds(UserRequested.Id, VerseTarget.Id);

                if (!LikeableVerseCommentIds.Contains(CommentLiked.Id))
                {
                    _logger.LogWarning($"Unauthorized, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Comment: [Id: {CommentLiked.Id}, CommentOwnerUsername: {CommentLiked.User.UserName}] on Verse: [Id: {VerseTarget.Id}] User does not have permission to like.");
                    return Unauthorized(new { Message = "You do not have permission to attach comment to this note" });
                }

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

                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed.  User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has successfully liked Comment: [Id: {CommentLiked.Id}, CommentOwnerUsername: {CommentLiked.User.UserName}] on Verse: [Id: {VerseTarget.Id}]");
                return Ok(new { Message = "You have successfully liked the comment!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to like Comment: [Id: {CommentLiked?.Id}, CommentOwnerUsername: {CommentLiked?.User.UserName}] on Verse: [Id: {VerseTarget?.Id}]. Error Details: {ex}");
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

            Like? LikeDeleted = null;

            try
            {
                LikeDeleted = await _db.Like.FirstOrDefaultAsync(l => l.UserId == UserRequested.Id && l.LikeNote != null && l.LikeNote.NoteId == model.NoteId);

                if (LikeDeleted == null)
                {
                    _logger.LogWarning($"Not Found, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to remove on Note: [model.NoteId: {model.NoteId}]");
                    return NotFound(new { Message = "There is no 'like' attached this note." });
                }

                _db.Like.Remove(LikeDeleted);

                await _db.SaveChangesAsync();

                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has removed his Like: [Id: {LikeDeleted.Id}] on Note");
                return Ok(new { Message = "Like that attached this note is successfully deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to remove Like: [Id: {LikeDeleted?.Id}] on Note. Error Details: {ex}");
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


            Like? LikeDeleted = null;

            try
            {
                LikeDeleted = await _db.Like.FirstOrDefaultAsync(l => l.UserId == UserRequested.Id && l.LikeComment != null && l.LikeComment.CommentId == model.CommentId);

                if (LikeDeleted == null)
                {
                    _logger.LogWarning($"Not Found, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to remove on Claimed Comment: [model.CommentId: {model.CommentId}]");
                    return NotFound(new { Message = "There is no 'like' attached this comment." });
                }

                _db.Like.Remove(LikeDeleted);


                await _db.SaveChangesAsync();
                _logger.LogInformation($"Operation completed. User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has removed his Like: [Id: {LikeDeleted.Id}] on Comment");

                return Ok(new { Message = "Like that attached this comment is successfully deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] is trying to remove Like: [Id: {LikeDeleted?.Id}] on Comment. Error Details: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}
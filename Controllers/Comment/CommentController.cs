using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.CommentHandler
{

    [ApiController, Route("comment"), Authorize]
    public class CommentController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpGet, Route("")]
        public async Task<IActionResult> GetComments()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            HashSet<Guid> FollowedUserIds = _db.Follow
              .Where(f => f.FollowerId == UserRequested.Id)
              .Select(f => f.FollowedId)
              .ToHashSet();


            List<CommentParentCommentDTO>? data = await _db.Comment
                                            .Where(c => c.UserId == UserRequested.Id)
                                            .Select(c => c.ToCommentParentUserDTO(c.ParentComment != null && FollowedUserIds.Contains(c.ParentComment.UserId))) //Does UserRequest follow the parent comment owner?
                                            .ToListAsync();
            return Ok(new { data });
        }

        [HttpGet, Route("note/{NoteId}")]
        public async Task<IActionResult> GetCommentFromNote([FromRoute] GetCommentsFromNoteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            var data = "Will be implemented";
            //TODO: Will be implemented

            return Ok(new { data });
        }

        [HttpGet, Route("verse/{VerseId}")]
        public async Task<IActionResult> GetCommentFromVerse([FromRoute] GetCommentsFromVerseModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            var data = "Will be implemented";
            //TODO: Will be implemented

            return Ok(new { data });
        }

        [HttpPost, Route("create/note")]
        public async Task<IActionResult> CreateCommentOnNote([FromRoute] EntityCommentCreateModel model)
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


            Comment CommentCreated = new()
            {
                Text = model.CommentText,
                UserId = UserRequested.Id,
            };

            _db.Comment.Add(CommentCreated);

            CommentNote CommentNoteCreated = new()
            {
                Comment = CommentCreated,
                NoteId = NoteTarget.Id
            };

            _db.CommentNote.Add(CommentNoteCreated);

            try
            {

                //TODO: Will be implemented

                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have created comment on that note successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPost, Route("create/verse")]
        public async Task<IActionResult> CreateCommentOnVerse([FromRoute] EntityCommentCreateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Verse? VerseTarget = await _db.Verse.FirstOrDefaultAsync(v => v.Id == model.EntityId);

            if (VerseTarget == null)
                return NotFound(new { Message = "Verse not found." });

            Comment CommentCreated = new()
            {
                Text = model.CommentText,
                UserId = UserRequested.Id,
            };

            _db.Comment.Add(CommentCreated);

            CommentVerse CommentVerseCreated = new()
            {
                Comment = CommentCreated,
                VerseId = VerseTarget.Id
            };

            _db.CommentVerse.Add(CommentVerseCreated);

            try
            {
                //TODO: Will be implemented

                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have created comment on that verse successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateComment([FromRoute] CommentUpdateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Comment? CommentUpdated = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.UserId == UserRequested.Id);

            if (CommentUpdated == null)
                return NotFound(new { Message = "Comment not found." });

            CommentUpdated.Text = model.NewCommentText;
            CommentUpdated.UpdatedAt = DateTime.UtcNow;

            _db.Comment.Update(CommentUpdated);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have updated the comment!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> DeleteComment([FromRoute] CommentDeleteModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Comment? CommentDeleted = await _db.Comment.FirstOrDefaultAsync(c => c.Id == model.CommentId && c.UserId == UserRequested.Id);

            if (CommentDeleted == null)
                return NotFound(new { Message = "Comment not found." });

            _db.Comment.Remove(CommentDeleted);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "You have deleted the comment successfully" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }
    }
}
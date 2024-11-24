using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.NoteHandler
{

    [ApiController, Route("note"), Authorize]
    public class NoteController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        [HttpGet, Route("")]
        public async Task<IActionResult> GetNote()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            List<NoteDTO> data = await _db.Note.Where(n => n.UserId == UserRequested.Id).Select(n => n.ToNoteDTO()).ToListAsync();

            return Ok(new { data });
        }



        [HttpGet, Route("{ScriptureNumber}/{SectionNumber}/{ChapterNumber}/{VerseNumber}")]
        public async Task<IActionResult> GetNote([FromBody] VerseValidatedModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            var VerseAttached = await _db.Verse
                            .Where(v => v.Number == model.VerseNumber &&
                                    v.Chapter.Number == model.ChapterNumber &&
                                    v.Chapter.Section.Number == model.SectionNumber &&
                                    v.Chapter.Section.Scripture.Number == model.ScriptureNumber)
                                    .Select(v => new { v.Id })
                                    .FirstOrDefaultAsync();

            if (VerseAttached == null)
                return NotFound(new { Message = "Verse not found." });

            List<Note> data = await _db.Note
                .Where(n => n.VerseId == VerseAttached.Id &&
                            _db.Follow.Any(f => //Notes attached on specified verse and whose UserRequested follows.
                                f.FollowerId == UserRequested.Id &&
                                f.FollowedId == n.UserId &&
                                f.Status == FollowStatus.Accepted))
                            .ToListAsync();

            try
            {
                return Ok(new { data });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }



        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Verse? VerseAttached = await _db.Verse.FirstOrDefaultAsync(v => v.Number == model.Verse.VerseNumber && v.Chapter.Number == model.Verse.ChapterNumber && v.Chapter.Section.Number == model.Verse.SectionNumber && v.Chapter.Section.Scripture.Number == model.Verse.ScriptureNumber);

            if (VerseAttached == null)
                return NotFound(new { Message = "Verse not found." });

            //TODO: Add "note per verse" constraint.

            Note NoteCreated = new()
            {
                Text = model.NoteText,
                UserId = UserRequested.Id,
                VerseId = VerseAttached.Id
            };

            _db.Note.Add(NoteCreated);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Note is successfully created!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }


        [HttpPut, Route("update")]
        public async Task<IActionResult> UpdateNote([FromBody] NoteUpdateModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Note? NoteUpdated = await _db.Note.FirstOrDefaultAsync(n => n.UserId == UserRequested.Id && n.Id == model.NoteId);

            if (NoteUpdated == null)
                return NotFound(new { Message = "Note not found." });

            NoteUpdated.Text = model.NewNoteText;
            NoteUpdated.UpdatedAt = DateTime.UtcNow;

            _db.Note.Update(NoteUpdated);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Note is successfully updated!" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> DeleteNote([FromBody] NoteIdentifierModel model)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Note? NoteDeleted = await _db.Note.FirstOrDefaultAsync(n => n.UserId == UserRequested.Id && n.Id == model.NoteId);

            if (NoteDeleted == null)
                return NotFound(new { Message = "Note not found!" });


            _db.Note.Remove(NoteDeleted);

            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Note is successfully deleted" });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}
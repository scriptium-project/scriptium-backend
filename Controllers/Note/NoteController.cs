using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.NoteHandler
{

    [ApiController, Route("note"), Authorize, EnableRateLimiting(policyName: "InteractionControllerRateLimit")]
    public class NoteController(ApplicationDBContext db, UserManager<User> userManager, ILogger<NoteController> logger) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ILogger<NoteController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet, Route("")]
        public async Task<IActionResult> GetNote()
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });
            try
            {
                List<NoteDTO> data = await _db.Note.Where(n => n.UserId == UserRequested.Id).Select(n => n.ToNoteDTO()).ToListAsync();

                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has demanded his note records. {data.Count} row has ben returned.");
                return Ok(new { data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] trying to get note records. Error Details: {ex}");
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

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
            Verse? VerseAttached = null;
            try
            {
                VerseAttached = await _db.Verse
                                .Where(v => v.Number == model.VerseNumber &&
                                        v.Chapter.Number == model.ChapterNumber &&
                                        v.Chapter.Section.Number == model.SectionNumber &&
                                        v.Chapter.Section.Scripture.Number == model.ScriptureNumber)
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

                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has demanded his note records attached on Verse: [Id: {VerseAttached?.Id}]. {data.Count} row has ben returned.");
                return Ok(new { data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] trying to get note records attached on Verse: [Id: {VerseAttached?.Id}]. Error Details: {ex}");
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

            Verse? VerseAttached = null;
            try
            {
                VerseAttached = await _db.Verse.FirstOrDefaultAsync(v => v.Number == model.Verse.VerseNumber && v.Chapter.Number == model.Verse.ChapterNumber && v.Chapter.Section.Number == model.Verse.SectionNumber && v.Chapter.Section.Scripture.Number == model.Verse.ScriptureNumber);

                if (VerseAttached == null)
                    return NotFound(new { Message = "Verse not found." });


                Note NoteCreated = new()
                {
                    Text = model.NoteText,
                    UserId = UserRequested.Id,
                    VerseId = VerseAttached.Id
                };

                _db.Note.Add(NoteCreated);


                await _db.SaveChangesAsync();
                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has created note on Verse: [Id: {VerseAttached?.Id}].");
                return Ok(new { Message = "Note is successfully created!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] creating a note on Verse: [Id: {VerseAttached?.Id}]. Error Details: {ex}");
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

            Note? NoteUpdated = null;

            try
            {
                NoteUpdated = await _db.Note.FirstOrDefaultAsync(n => n.UserId == UserRequested.Id && n.Id == model.NoteId);

                if (NoteUpdated == null)
                    return NotFound(new { Message = "Note not found." });


                NoteUpdated.Text = model.NewNoteText;
                NoteUpdated.UpdatedAt = DateTime.UtcNow;

                _db.Note.Update(NoteUpdated);

                await _db.SaveChangesAsync();
                _logger.LogInformation($"Operation completed: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] has updating Note: [Id: {NoteUpdated?.Id}].");

                return Ok(new { Message = "Note is successfully updated!" });
            }
            catch (Exception)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] updating a Note: [Id: {NoteUpdated?.Id}]");
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

            Note? NoteDeleted = null;

            try
            {
                NoteDeleted = await _db.Note.FirstOrDefaultAsync(n => n.UserId == UserRequested.Id && n.Id == model.NoteId);

                if (NoteDeleted == null)
                    return NotFound(new { Message = "Note not found!" });

                _db.Note.Remove(NoteDeleted);

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Note is successfully deleted" });
            }
            catch (Exception)
            {
                _logger.LogError($"Error occurred, while: User: [Id: {UserRequested.Id}, Username: {UserRequested.UserName}] deleting a Note: [Id: {NoteDeleted?.Id}]");

                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }
        }
    }
}
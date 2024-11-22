using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.Controllers.Validation;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.SavingHandler
{

    [ApiController, Route("saving"), Authorize]
    public class SavingController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {

        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpPost, Route("save")]
        public async Task<IActionResult> Save([FromBody] SavingProcessModel model)
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


            //This lists will indicate users which insertion has been succeed or failed.
            List<CollectionProcessResultDTO> succeed = [];
            List<CollectionProcessResultDTO> failed = [];


            foreach (string CollectionName in model.CollectionNames)
            {
                CollectionProcessResultDTO DTO;

                Collection? Collection = await _db.Collection
                    .FirstOrDefaultAsync(c => c.Name == CollectionName && c.UserId == UserRequested.Id);

                if (Collection == null)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionName);
                    failed.Add(DTO);
                    continue;
                }

                bool isAlreadyExists = await _db.CollectionVerse.AnyAsync(cv => cv.CollectionId == Collection.Id && cv.VerseId == VerseAttached.Id);

                if (isAlreadyExists)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.AlreadyDone);

                    succeed.Add(DTO);
                    continue;
                }


                //All non-succeed conditions were passed. So we are ready to make process.

                CollectionVerse collectionVerse = new()
                {
                    CollectionId = Collection.Id,
                    VerseId = VerseAttached.Id
                };

                _db.CollectionVerse.Add(collectionVerse);

                try
                {
                    await _db.SaveChangesAsync();

                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.Succeed);

                    succeed.Add(DTO);
                }
                catch (Exception)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.Error);

                    failed.Add(DTO);
                    _db.Entry(collectionVerse).State = EntityState.Detached;
                }
            }

            var data = new
            {
                succeed,
                failed
            };

            return Ok(new { success = succeed.Count > 0, data });
        }

        [HttpDelete, Route("unsave")]
        public async Task<IActionResult> Unsave([FromBody] SavingProcessModel model)
        {
            CollectionProcessResultDTO DTO;

            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            Verse? VerseRemoved = await _db.Verse.FirstOrDefaultAsync(v => v.Number == model.Verse.VerseNumber && v.Chapter.Number == model.Verse.ChapterNumber && v.Chapter.Section.Number == model.Verse.SectionNumber && v.Chapter.Section.Scripture.Number == model.Verse.ScriptureNumber);

            if (VerseRemoved == null)
                return NotFound(new { Message = "Verse not found." });

            //This lists will indicate users which insertion has been succeed or failed.
            List<CollectionProcessResultDTO> succeed = [];
            List<CollectionProcessResultDTO> failed = [];


            foreach (string CollectionName in model.CollectionNames)
            {
                // Check if the collection exists for the user
                var Collection = await _db.Collection
                    .FirstOrDefaultAsync(c => c.Name == CollectionName && c.UserId == UserRequested.Id);

                if (Collection == null)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionName);

                    failed.Add(DTO);
                    continue;
                }

                // Check if the verse exists in the collection
                CollectionVerse? CollectionVerse = await _db.CollectionVerse
                    .FirstOrDefaultAsync(cv => cv.CollectionId == Collection.Id && cv.VerseId == VerseRemoved.Id);

                if (CollectionVerse == null)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.NotFound);

                    failed.Add(DTO);
                    continue;
                }

                //All non-succeed conditions were passed. So we are ready to make process.

                try
                {
                    _db.CollectionVerse.Remove(CollectionVerse);
                    await _db.SaveChangesAsync();

                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.Succeed);
                    succeed.Add(DTO);
                }
                catch (Exception)
                {
                    DTO = Collection.GetCollectionProcessResultDTO(CollectionStatus.Error);

                    failed.Add(DTO);
                }
            }

            var data = new
            {
                succeed,
                failed
            };

            return Ok(new { success = succeed.Count > 0, data });
        }

    }
}